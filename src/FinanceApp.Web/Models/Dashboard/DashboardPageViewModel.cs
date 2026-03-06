using FinanceApp.Application.DTOs.Dashboard;
using FinanceApp.Application.DTOs.Insights;

namespace FinanceApp.Web.Models.Dashboard;

public class DashboardPageViewModel
{
    public DashboardAnalyticsDto Analytics { get; init; } = new();
    public IReadOnlyCollection<InsightDto> Insights { get; init; } = Array.Empty<InsightDto>();
}
