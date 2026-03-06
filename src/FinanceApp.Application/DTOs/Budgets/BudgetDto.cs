namespace FinanceApp.Application.DTOs.Budgets;

public record BudgetDto(Guid Id, Guid CategoryId, string CategoryName, int Year, int Month, decimal PlannedAmount, decimal SpentAmount, decimal Remaining);
