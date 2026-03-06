using FinanceApp.Application.DTOs;

namespace FinanceApp.Application.DTOs.Dashboard;

public class DashboardAnalyticsDto
{
    public DashboardSummaryDto Summary { get; init; } = new(0,0,0,0);
    public DashboardSummaryDto PreviousSummary { get; init; } = new(0,0,0,0);
    public IReadOnlyCollection<CategoryExpenseDto> ExpenseByCategory { get; init; } = Array.Empty<CategoryExpenseDto>();
    public IReadOnlyCollection<MonthlyTrendPointDto> MonthlyTrend { get; init; } = Array.Empty<MonthlyTrendPointDto>();
    public IReadOnlyCollection<CashFlowPointDto> CashFlowByDay { get; init; } = Array.Empty<CashFlowPointDto>();
}
