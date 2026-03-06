using FinanceApp.Domain.Common;
using FinanceApp.Domain.Enums;

namespace FinanceApp.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid? AccountId { get; private set; }
    public Guid? CreditCardId { get; private set; }
    public TransactionType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public DateTime? PostedDate { get; private set; }
    public Guid? CategoryId { get; private set; }
    public Guid? SubcategoryId { get; private set; }
    public string? ExternalId { get; private set; }
    public string Source { get; private set; } = "Manual";
    public bool IsRecurring { get; private set; }
    public bool IsSubscriptionCandidate { get; private set; }
    public Guid? ImportBatchId { get; private set; }
    public string? Notes { get; private set; }

    public Account? Account { get; private set; }
    public CreditCard? CreditCard { get; private set; }

    private Transaction() { }

    public Transaction(Guid userId, TransactionType type, string description, decimal amount, DateTime transactionDate)
    {
        UserId = userId;
        Type = type;
        Description = description;
        Amount = amount;
        TransactionDate = transactionDate;
    }
}
