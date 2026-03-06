using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Transactions;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Transactions;

public class TransactionCrudService : ITransactionCrudService
{
    private readonly IAppDbContext _context;

    public TransactionCrudService(IAppDbContext context) => _context = context;

    public async Task<IReadOnlyCollection<TransactionDto>> ListAsync(Guid userId, DateTime? from, DateTime? to, string? search, CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions.Where(x => x.UserId == userId);

        if (from.HasValue) query = query.Where(x => x.TransactionDate >= from.Value.Date);
        if (to.HasValue) query = query.Where(x => x.TransactionDate < to.Value.Date.AddDays(1));
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Description.ToLower().Contains(search.ToLower()));

        return await query
            .OrderByDescending(x => x.TransactionDate)
            .Take(300)
            .Select(x => new TransactionDto(x.Id, x.TransactionDate, x.Description, x.Amount, x.Type, x.AccountId, x.CreditCardId, x.CategoryId, x.IsRecurring))
            .ToListAsync(cancellationToken);
    }

    public async Task<TransactionDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(x => x.UserId == userId && x.Id == id)
            .Select(x => new TransactionDto(x.Id, x.TransactionDate, x.Description, x.Amount, x.Type, x.AccountId, x.CreditCardId, x.CategoryId, x.IsRecurring))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(Guid userId, UpsertTransactionDto dto, CancellationToken cancellationToken = default)
    {
        var transaction = new Transaction(userId, dto.Type, dto.Description, dto.Amount, dto.TransactionDate);
        transaction.UpdateManual(dto.Type, dto.Description, dto.Amount, dto.TransactionDate, dto.AccountId, dto.CreditCardId, dto.CategoryId, dto.Notes, dto.IsRecurring);

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return transaction.Id;
    }

    public async Task<bool> UpdateAsync(Guid userId, Guid id, UpsertTransactionDto dto, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (transaction is null) return false;

        transaction.UpdateManual(dto.Type, dto.Description, dto.Amount, dto.TransactionDate, dto.AccountId, dto.CreditCardId, dto.CategoryId, dto.Notes, dto.IsRecurring);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (transaction is null) return false;

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
