using FinanceApp.Domain.Enums;

namespace FinanceApp.Application.DTOs;

public class ImportExecutionResultDto
{
    public Guid BatchId { get; init; }
    public string FileName { get; init; } = string.Empty;
    public ImportBatchStatus Status { get; init; }
    public int TotalRecords { get; init; }
    public int ImportedRecords { get; init; }
    public int DuplicatedRecords { get; init; }
    public int FailedRecords { get; init; }
    public List<string> Warnings { get; init; } = new();
}
