using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Web.Models.Dashboard;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardAnalyticsService _analyticsService;
    private readonly IInsightService _insightService;

    public DashboardController(IDashboardAnalyticsService analyticsService, IInsightService insightService)
    {
        _analyticsService = analyticsService;
        _insightService = insightService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var reference = DateTime.UtcNow;

        var analytics = await _analyticsService.GetAsync(userId, reference, cancellationToken);
        var insights = await _insightService.GetInsightsAsync(userId, reference, cancellationToken);

        return View(new DashboardPageViewModel
        {
            Analytics = analytics,
            Insights = insights
        });
    }
}
