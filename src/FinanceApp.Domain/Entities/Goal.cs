using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class Goal : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal TargetAmount { get; private set; }
    public decimal CurrentAmount { get; private set; }
    public DateTime? TargetDate { get; private set; }

    private Goal() { }

    public Goal(Guid userId, string name, decimal targetAmount)
    {
        UserId = userId;
        Name = name;
        TargetAmount = targetAmount;
    }

    public void Update(string name, decimal targetAmount, decimal currentAmount, DateTime? targetDate)
    {
        Name = name;
        TargetAmount = targetAmount;
        CurrentAmount = currentAmount;
        TargetDate = targetDate;
        Touch();
    }
}
