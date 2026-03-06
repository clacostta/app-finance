namespace FinanceApp.Application.DTOs;

public record OfxTransactionDto(
    string? ExternalId,
    DateTime TransactionDate,
    DateTime? PostedDate,
    decimal Amount,
    string Description,
    string Type,
    string? AccountNumber,
    string? CardNumber);
