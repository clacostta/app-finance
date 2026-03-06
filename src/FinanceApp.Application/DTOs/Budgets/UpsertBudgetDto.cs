namespace FinanceApp.Application.DTOs.Budgets;

public class UpsertBudgetDto
{
    public Guid CategoryId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal PlannedAmount { get; set; }
}
