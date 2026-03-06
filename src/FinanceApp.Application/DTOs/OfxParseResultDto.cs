namespace FinanceApp.Application.DTOs;

public class OfxParseResultDto
{
    public string? AccountId { get; init; }
    public string? BankId { get; init; }
    public List<OfxTransactionDto> Transactions { get; init; } = new();
    public List<string> Warnings { get; init; } = new();
}
