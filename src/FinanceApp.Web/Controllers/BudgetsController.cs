using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Budgets;
using FinanceApp.Web.Models.Budgets;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class BudgetsController : Controller
{
    private readonly IBudgetService _service;
    private readonly ICategoryCrudService _categoryService;
    private readonly INotificationService _notificationService;

    public BudgetsController(IBudgetService service, ICategoryCrudService categoryService, INotificationService notificationService)
    {
        _service = service;
        _categoryService = categoryService;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index(int? year, int? month, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var y = year ?? DateTime.Today.Year;
        var m = month ?? DateTime.Today.Month;

        var items = await _service.ListAsync(userId, y, m, cancellationToken);
        ViewBag.Year = y;
        ViewBag.Month = m;
        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadCategories(cancellationToken);
        return View(new BudgetFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BudgetFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategories(cancellationToken);
            return View(model);
        }

        var userId = UserIdResolver.Resolve(User);
        await _service.CreateAsync(userId, new UpsertBudgetDto { CategoryId = model.CategoryId, Year = model.Year, Month = model.Month, PlannedAmount = model.PlannedAmount }, cancellationToken);
        return RedirectToAction(nameof(Index), new { year = model.Year, month = model.Month });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var item = await _service.GetAsync(userId, id, cancellationToken);
        if (item is null) return NotFound();

        await LoadCategories(cancellationToken);
        return View(new BudgetFormViewModel { Id = item.Id, CategoryId = item.CategoryId, Year = item.Year, Month = item.Month, PlannedAmount = item.PlannedAmount });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BudgetFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || model.Id is null)
        {
            await LoadCategories(cancellationToken);
            return View(model);
        }

        var userId = UserIdResolver.Resolve(User);
        await _service.UpdateAsync(userId, model.Id.Value, new UpsertBudgetDto { CategoryId = model.CategoryId, Year = model.Year, Month = model.Month, PlannedAmount = model.PlannedAmount }, cancellationToken);
        return RedirectToAction(nameof(Index), new { year = model.Year, month = model.Month });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, int year, int month, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        await _service.DeleteAsync(userId, id, cancellationToken);
        return RedirectToAction(nameof(Index), new { year, month });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateAlerts(int year, int month, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        await _notificationService.GenerateBudgetAlertsAsync(userId, new DateTime(year, month, 1), cancellationToken);
        return RedirectToAction(nameof(Index), new { year, month });
    }

    private async Task LoadCategories(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        ViewBag.Categories = await _categoryService.ListAsync(userId, cancellationToken);
    }
}
