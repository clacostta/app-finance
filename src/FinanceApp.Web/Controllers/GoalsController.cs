using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Goals;
using FinanceApp.Web.Models.Goals;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class GoalsController : Controller
{
    private readonly IGoalService _service;

    public GoalsController(IGoalService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var items = await _service.ListAsync(userId, cancellationToken);
        return View(items);
    }

    [HttpGet]
    public IActionResult Create() => View(new GoalFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GoalFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = UserIdResolver.Resolve(User);
        await _service.CreateAsync(userId, new UpsertGoalDto { Name = model.Name, TargetAmount = model.TargetAmount, CurrentAmount = model.CurrentAmount, TargetDate = model.TargetDate }, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var item = await _service.GetAsync(userId, id, cancellationToken);
        if (item is null) return NotFound();

        return View(new GoalFormViewModel { Id = item.Id, Name = item.Name, TargetAmount = item.TargetAmount, CurrentAmount = item.CurrentAmount, TargetDate = item.TargetDate });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(GoalFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || model.Id is null) return View(model);

        var userId = UserIdResolver.Resolve(User);
        await _service.UpdateAsync(userId, model.Id.Value, new UpsertGoalDto { Name = model.Name, TargetAmount = model.TargetAmount, CurrentAmount = model.CurrentAmount, TargetDate = model.TargetDate }, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        await _service.DeleteAsync(userId, id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
