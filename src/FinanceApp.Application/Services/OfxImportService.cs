using System.Security.Cryptography;
using System.Text;
using FinanceApp.Application.Abstractions;
using FinanceApp.Application.DTOs;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services;

public class OfxImportService : IOfxImportService
{
    private readonly IAppDbContext _context;
    private readonly IOfxParser _ofxParser;

    public OfxImportService(IAppDbContext context, IOfxParser ofxParser)
    {
        _context = context;
        _ofxParser = ofxParser;
    }

    public async Task<ImportPreviewDto> PreviewAsync(Guid userId, string fileName, Stream contentStream, CancellationToken cancellationToken = default)
    {
        var content = await ReadStreamAsync(contentStream, cancellationToken);
        var fileHash = ComputeSha256(content);

        var parse = _ofxParser.Parse(content);
        var existing = await _context.Transactions
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .Select(t => new { t.TransactionDate, t.Amount, t.Description, t.ExternalId })
            .ToListAsync(cancellationToken);

        var duplicated = new List<OfxTransactionDto>();
        var fresh = new List<OfxTransactionDto>();

        foreach (var tx in parse.Transactions)
        {
            var isDup = existing.Any(e =>
                (!string.IsNullOrWhiteSpace(tx.ExternalId) && e.ExternalId == tx.ExternalId) ||
                (e.TransactionDate.Date == tx.TransactionDate.Date
                    && e.Amount == tx.Amount
                    && e.Description.ToLowerInvariant() == tx.Description.ToLowerInvariant()));

            if (isDup) duplicated.Add(tx);
            else fresh.Add(tx);
        }

        return new ImportPreviewDto
        {
            FileName = fileName,
            FileHash = fileHash,
            TotalRecords = parse.Transactions.Count,
            NewRecords = fresh.Count,
            DuplicatedRecords = duplicated.Count,
            NewTransactions = fresh,
            DuplicatedTransactions = duplicated,
            Warnings = parse.Warnings
        };
    }

    public async Task<ImportExecutionResultDto> ImportAsync(Guid userId, string fileName, Stream contentStream, CancellationToken cancellationToken = default)
    {
        var preview = await PreviewAsync(userId, fileName, contentStream, cancellationToken);
        var existingBatch = await _context.ImportBatches
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.FileHash == preview.FileHash)
            .OrderByDescending(x => x.ImportedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingBatch is not null && existingBatch.Status is ImportBatchStatus.Completed or ImportBatchStatus.CompletedWithWarnings)
        {
            return new ImportExecutionResultDto
            {
                BatchId = existingBatch.Id,
                FileName = existingBatch.FileName,
                Status = existingBatch.Status,
                TotalRecords = existingBatch.TotalRecords,
                ImportedRecords = existingBatch.ImportedRecords,
                DuplicatedRecords = existingBatch.DuplicatedRecords,
                FailedRecords = existingBatch.FailedRecords,
                Warnings = preview.Warnings
            };
        }

        var batch = new ImportBatch(userId, fileName, preview.FileHash);
        batch.StartProcessing();

        _context.ImportBatches.Add(batch);
        await _context.SaveChangesAsync(cancellationToken);

        var imported = 0;
        var failed = 0;

        try
        {
            foreach (var tx in preview.NewTransactions)
            {
                try
                {
                    var transactionType = tx.Amount >= 0 ? TransactionType.Income : TransactionType.Expense;
                    var transaction = new Transaction(userId, transactionType, tx.Description, Math.Abs(tx.Amount), tx.TransactionDate);
                    transaction.SetImportMetadata(null, null, tx.PostedDate, tx.ExternalId, batch.Id, "OFX");
                    _context.Transactions.Add(transaction);
                    imported++;
                }
                catch
                {
                    failed++;
                }
            }

            _context.ImportedFiles.Add(new ImportedFile(batch.Id, $"imports/{preview.FileHash}.ofx", fileName, 0));

            batch.Complete(preview.TotalRecords, imported, preview.DuplicatedRecords, failed);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            batch.Fail(ex.Message);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return new ImportExecutionResultDto
        {
            BatchId = batch.Id,
            FileName = fileName,
            Status = batch.Status,
            TotalRecords = preview.TotalRecords,
            ImportedRecords = imported,
            DuplicatedRecords = preview.DuplicatedRecords,
            FailedRecords = failed,
            Warnings = preview.Warnings
        };
    }

    public async Task<IReadOnlyCollection<ImportHistoryItemDto>> GetHistoryAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.ImportBatches
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.ImportedAt)
            .Select(x => new ImportHistoryItemDto(
                x.Id,
                x.FileName,
                x.ImportedAt,
                x.Status,
                x.TotalRecords,
                x.ImportedRecords,
                x.DuplicatedRecords,
                x.FailedRecords))
            .ToListAsync(cancellationToken);
    }

    private static async Task<string> ReadStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        using var reader = new StreamReader(stream, Encoding.UTF8, true, leaveOpen: true);
        var content = await reader.ReadToEndAsync();

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        return content;
    }

    private static string ComputeSha256(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
