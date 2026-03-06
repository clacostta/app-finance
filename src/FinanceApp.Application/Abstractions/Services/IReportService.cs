using FinanceApp.Application.DTOs.Reports;

namespace FinanceApp.Application.Abstractions.Services;

public interface IReportService
{
    Task<ReportBundleDto> GetBundleAsync(Guid userId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
}
