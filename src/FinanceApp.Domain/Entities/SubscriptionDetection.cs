using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class SubscriptionDetection : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Merchant { get; private set; } = string.Empty;
    public decimal AverageAmount { get; private set; }
    public int OccurrenceCount { get; private set; }
    public bool IsActive { get; private set; } = true;

    private SubscriptionDetection() { }

    public SubscriptionDetection(Guid userId, string merchant, decimal averageAmount, int occurrenceCount)
    {
        UserId = userId;
        Merchant = merchant;
        AverageAmount = averageAmount;
        OccurrenceCount = occurrenceCount;
    }
}
