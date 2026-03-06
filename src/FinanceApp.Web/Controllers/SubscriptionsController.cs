using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class SubscriptionsController : Controller
{
    private readonly ISubscriptionService _service;

    public SubscriptionsController(ISubscriptionService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var items = await _service.ListAsync(userId, cancellationToken);
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        await _service.RefreshDetectionsAsync(userId, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        await _service.DeactivateAsync(userId, id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
