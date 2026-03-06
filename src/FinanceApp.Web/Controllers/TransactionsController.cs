using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Transactions;
using FinanceApp.Domain.Enums;
using FinanceApp.Web.Models.Transactions;
using FinanceApp.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Web.Controllers;

[Authorize]
public class TransactionsController : Controller
{
    private readonly ITransactionCrudService _service;
    private readonly IAccountCrudService _accountService;
    private readonly ICreditCardCrudService _cardService;
    private readonly ICategoryCrudService _categoryService;

    public TransactionsController(ITransactionCrudService service, IAccountCrudService accountService, ICreditCardCrudService cardService, ICategoryCrudService categoryService)
    {
        _service = service;
        _accountService = accountService;
        _cardService = cardService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(DateTime? from, DateTime? to, string? search, int page = 1, int pageSize = 25, CancellationToken cancellationToken = default)
    {
        var userId = UserIdResolver.Resolve(User);
        var total = await _service.CountAsync(userId, from, to, search, cancellationToken);
        var items = await _service.ListAsync(userId, from, to, search, page, pageSize, cancellationToken);

        ViewBag.FilterFrom = from?.ToString("yyyy-MM-dd");
        ViewBag.FilterTo = to?.ToString("yyyy-MM-dd");
        ViewBag.FilterSearch = search;
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        ViewBag.Total = total;
        ViewBag.TotalPages = (int)Math.Ceiling(total / (double)Math.Max(1, pageSize));

        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateLookups(cancellationToken);
        return View(new TransactionFormViewModel { Type = TransactionType.Expense });
    }

    [HttpPost]
    public async Task<IActionResult> Create(TransactionFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateLookups(cancellationToken);
            return View(model);
        }

        var userId = UserIdResolver.Resolve(User);
        await _service.CreateAsync(userId, Map(model), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        var item = await _service.GetAsync(userId, id, cancellationToken);
        if (item is null) return NotFound();

        await PopulateLookups(cancellationToken);
        return View(new TransactionFormViewModel
        {
            Id = item.Id,
            TransactionDate = item.TransactionDate,
            Description = item.Description,
            Amount = item.Amount,
            Type = item.Type,
            AccountId = item.AccountId,
            CreditCardId = item.CreditCardId,
            CategoryId = item.CategoryId,
            IsRecurring = item.IsRecurring
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TransactionFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid || model.Id is null)
        {
            await PopulateLookups(cancellationToken);
            return View(model);
        }

        var userId = UserIdResolver.Resolve(User);
        var ok = await _service.UpdateAsync(userId, model.Id.Value, Map(model), cancellationToken);
        if (!ok) return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        await _service.DeleteAsync(userId, id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private UpsertTransactionDto Map(TransactionFormViewModel m) => new()
    {
        TransactionDate = m.TransactionDate,
        Description = m.Description,
        Amount = m.Amount,
        Type = m.Type,
        AccountId = m.AccountId,
        CreditCardId = m.CreditCardId,
        CategoryId = m.CategoryId,
        Notes = m.Notes,
        IsRecurring = m.IsRecurring
    };

    private async Task PopulateLookups(CancellationToken cancellationToken)
    {
        var userId = UserIdResolver.Resolve(User);
        ViewBag.Accounts = await _accountService.ListAsync(userId, cancellationToken);
        ViewBag.CreditCards = await _cardService.ListAsync(userId, cancellationToken);
        ViewBag.Categories = await _categoryService.ListAsync(userId, cancellationToken);
    }
}
