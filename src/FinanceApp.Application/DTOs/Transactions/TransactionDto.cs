using FinanceApp.Domain.Enums;

namespace FinanceApp.Application.DTOs.Transactions;

public record TransactionDto(Guid Id, DateTime TransactionDate, string Description, decimal Amount, TransactionType Type, Guid? AccountId, Guid? CreditCardId, Guid? CategoryId, bool IsRecurring);
