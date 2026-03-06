using FinanceApp.Application.DTOs.Reports;

namespace FinanceApp.Web.Models.Reports;

public class ReportsPageViewModel
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public ReportBundleDto Bundle { get; set; } = new();
}
