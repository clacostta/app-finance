using FinanceApp.Application.DTOs.Dashboard;

namespace FinanceApp.Application.Abstractions.Services;

public interface IDashboardAnalyticsService
{
    Task<DashboardAnalyticsDto> GetAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken = default);
}
