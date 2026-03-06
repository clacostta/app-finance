using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class NotificationsController : Controller
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
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
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        await _service.MarkAsReadAsync(userId, id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
