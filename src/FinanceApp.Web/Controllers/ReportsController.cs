using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Web.Models.Reports;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> Index(DateTime? from, DateTime? to, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var finalTo = to?.Date ?? DateTime.Today;
        var finalFrom = from?.Date ?? finalTo.AddMonths(-6);

        var bundle = await _reportService.GetBundleAsync(userId, finalFrom, finalTo, cancellationToken);
        return View(new ReportsPageViewModel
        {
            From = finalFrom,
            To = finalTo,
            Bundle = bundle
        });
    }
}
