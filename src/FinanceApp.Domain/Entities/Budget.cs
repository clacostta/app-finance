using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class Budget : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CategoryId { get; private set; }
    public int Year { get; private set; }
    public int Month { get; private set; }
    public decimal PlannedAmount { get; private set; }

    private Budget() { }

    public Budget(Guid userId, Guid categoryId, int year, int month, decimal plannedAmount)
    {
        UserId = userId;
        CategoryId = categoryId;
        Year = year;
        Month = month;
        PlannedAmount = plannedAmount;
    }
}
