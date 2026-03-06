using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class ImportedFile : BaseEntity
{
    public Guid ImportBatchId { get; private set; }
    public string StoragePath { get; private set; } = string.Empty;
    public string OriginalName { get; private set; } = string.Empty;
    public long SizeBytes { get; private set; }

    public ImportBatch ImportBatch { get; private set; } = default!;

    private ImportedFile() { }

    public ImportedFile(Guid importBatchId, string storagePath, string originalName, long sizeBytes)
    {
        ImportBatchId = importBatchId;
        StoragePath = storagePath;
        OriginalName = originalName;
        SizeBytes = sizeBytes;
    }
}
