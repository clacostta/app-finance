namespace FinanceApp.Application.DTOs.Reports;

public class ReportBundleDto
{
    public IReadOnlyCollection<ReportLineDto> ExpenseByCategory { get; init; } = Array.Empty<ReportLineDto>();
    public IReadOnlyCollection<ReportLineDto> IncomeByMonth { get; init; } = Array.Empty<ReportLineDto>();
    public IReadOnlyCollection<ReportLineDto> ExpenseByMonth { get; init; } = Array.Empty<ReportLineDto>();
    public IReadOnlyCollection<ReportLineDto> TopExpenseDescriptions { get; init; } = Array.Empty<ReportLineDto>();
}
