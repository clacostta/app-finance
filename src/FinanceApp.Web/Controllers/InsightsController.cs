using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Web.Models.Insights;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class InsightsController : Controller
{
    private readonly IInsightService _insightService;

    public InsightsController(IInsightService insightService)
    {
        _insightService = insightService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var insights = await _insightService.GetInsightsAsync(userId, DateTime.UtcNow, cancellationToken);
        return View(new InsightsPageViewModel { Items = insights });
    }
}
