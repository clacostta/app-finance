using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.CreditCards;
using FinanceApp.Web.Models.CreditCards;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class CreditCardsController : Controller
{
    private readonly ICreditCardCrudService _service;
    private readonly IAppDbContext _dbContext;

    public CreditCardsController(ICreditCardCrudService service, IAppDbContext dbContext)
    {
        _service = service;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var items = await _service.ListAsync(userId, cancellationToken);
        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateInstitutions(cancellationToken);
        return View(new CreditCardFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreditCardFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateInstitutions(cancellationToken);
            return View(model);
        }

        var userId = UserIdResolver.Resolve(User);
        await _service.CreateAsync(userId, new UpsertCreditCardDto
        {
            Name = model.Name,
            LimitAmount = model.LimitAmount,
            ClosingDay = model.ClosingDay,
            DueDay = model.DueDay,
            IsActive = model.IsActive,
            FinancialInstitutionId = model.FinancialInstitutionId
        }, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var item = await _service.GetAsync(userId, id, cancellationToken);
        if (item is null) return NotFound();

        await PopulateInstitutions(cancellationToken);
        return View(new CreditCardFormViewModel
        {
            Id = item.Id,
            Name = item.Name,
            LimitAmount = item.LimitAmount,
            ClosingDay = item.ClosingDay,
            DueDay = item.DueDay,
            IsActive = item.IsActive,
            FinancialInstitutionId = item.FinancialInstitutionId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CreditCardFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || model.Id is null)
        {
            await PopulateInstitutions(cancellationToken);
            return View(model);
        }

        var userId = UserIdResolver.Resolve(User);
        var updated = await _service.UpdateAsync(userId, model.Id.Value, new UpsertCreditCardDto
        {
            Name = model.Name,
            LimitAmount = model.LimitAmount,
            ClosingDay = model.ClosingDay,
            DueDay = model.DueDay,
            IsActive = model.IsActive,
            FinancialInstitutionId = model.FinancialInstitutionId
        }, cancellationToken);

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

    private async Task PopulateInstitutions(CancellationToken cancellationToken)
    {
        ViewBag.Institutions = await _dbContext.FinancialInstitutions
            .OrderBy(x => x.Name)
            .Select(x => new { x.Id, x.Name })
            .ToListAsync(cancellationToken);
    }
}
