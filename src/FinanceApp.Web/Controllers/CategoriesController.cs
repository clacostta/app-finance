using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Categories;
using FinanceApp.Web.Models.Categories;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly ICategoryCrudService _service;

    public CategoriesController(ICategoryCrudService service)
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
    public IActionResult Create() => View(new CategoryFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = UserIdResolver.Resolve(User);
        await _service.CreateAsync(userId, new UpsertCategoryDto { Name = model.Name }, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var item = await _service.GetAsync(userId, id, cancellationToken);
        if (item is null || item.IsSystemDefault) return NotFound();

        return View(new CategoryFormViewModel { Id = item.Id, Name = item.Name });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || model.Id is null) return View(model);

        var userId = UserIdResolver.Resolve(User);
        var updated = await _service.UpdateAsync(userId, model.Id.Value, new UpsertCategoryDto { Name = model.Name }, cancellationToken);
        if (!updated) return NotFound();

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
