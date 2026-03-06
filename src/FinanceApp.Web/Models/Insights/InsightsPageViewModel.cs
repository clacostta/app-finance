using FinanceApp.Application.DTOs.Insights;

namespace FinanceApp.Web.Models.Insights;

public class InsightsPageViewModel
{
    public IReadOnlyCollection<InsightDto> Items { get; set; } = Array.Empty<InsightDto>();
}
