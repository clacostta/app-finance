using FinanceApp.Application.DTOs.Insights;

namespace FinanceApp.Application.Abstractions.Services;

public interface IInsightService
{
    Task<IReadOnlyCollection<InsightDto>> GetInsightsAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken = default);
}
