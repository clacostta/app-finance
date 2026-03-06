namespace FinanceApp.Application.DTOs;

public record DashboardSummaryDto(
    decimal CurrentBalance,
    decimal MonthlyIncome,
    decimal MonthlyExpense,
    decimal MonthlySavings);
