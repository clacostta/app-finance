using FinanceApp.Domain.Enums;

namespace FinanceApp.Application.DTOs.Transactions;

public class UpsertTransactionDto
{
    public DateTime TransactionDate { get; set; } = DateTime.Today;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CreditCardId { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Notes { get; set; }
    public bool IsRecurring { get; set; }
}
