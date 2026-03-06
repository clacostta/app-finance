namespace FinanceApp.Application.DTOs;

public class ImportPreviewDto
{
    public string FileName { get; init; } = string.Empty;
    public string FileHash { get; init; } = string.Empty;
    public int TotalRecords { get; init; }
    public int NewRecords { get; init; }
    public int DuplicatedRecords { get; init; }
    public List<OfxTransactionDto> NewTransactions { get; init; } = new();
    public List<OfxTransactionDto> DuplicatedTransactions { get; init; } = new();
    public List<string> Warnings { get; init; } = new();
}
