using FinanceApp.Domain.Common;
using FinanceApp.Domain.Enums;

namespace FinanceApp.Domain.Entities;

public class ImportBatch : BaseEntity
{
    public Guid UserId { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public DateTime ImportedAt { get; private set; } = DateTime.UtcNow;
    public ImportBatchStatus Status { get; private set; } = ImportBatchStatus.Pending;
    public int TotalRecords { get; private set; }
    public int ImportedRecords { get; private set; }
    public int DuplicatedRecords { get; private set; }
    public int FailedRecords { get; private set; }
    public string FileHash { get; private set; } = string.Empty;
    public string? ErrorMessage { get; private set; }

    private ImportBatch() { }

    public ImportBatch(Guid userId, string fileName, string fileHash)
    {
        UserId = userId;
        FileName = fileName;
        FileHash = fileHash;
    }
}
