using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Budgets;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Budgets;

public class BudgetService : IBudgetService
{
    private readonly IAppDbContext _context;

    public BudgetService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<BudgetDto>> ListAsync(Guid userId, int year, int month, CancellationToken cancellationToken = default)
    {
        var items = await _context.Budgets
            .Where(x => x.UserId == userId && x.Year == year && x.Month == month)
            .ToListAsync(cancellationToken);

        var cats = await _context.TransactionCategories.ToDictionaryAsync(x => x.Id, x => x.Name, cancellationToken);

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);

        var spends = await _context.Transactions
            .Where(x => x.UserId == userId && x.Type == TransactionType.Expense && x.TransactionDate >= start && x.TransactionDate < end && x.CategoryId.HasValue)
            .GroupBy(x => x.CategoryId!.Value)
            .Select(g => new { CategoryId = g.Key, Total = g.Sum(x => x.Amount) })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Total, cancellationToken);

        return items.Select(x =>
        {
            var spent = spends.TryGetValue(x.CategoryId, out var s) ? s : 0m;
            return new BudgetDto(x.Id, x.CategoryId, cats.TryGetValue(x.CategoryId, out var name) ? name : "Categoria", x.Year, x.Month, x.PlannedAmount, spent, x.PlannedAmount - spent);
        }).OrderBy(x => x.CategoryName).ToList();
    }

    public async Task<BudgetDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (budget is null) return null;

        var cat = await _context.TransactionCategories.FirstOrDefaultAsync(x => x.Id == budget.CategoryId, cancellationToken);
        return new BudgetDto(budget.Id, budget.CategoryId, cat?.Name ?? "Categoria", budget.Year, budget.Month, budget.PlannedAmount, 0, budget.PlannedAmount);
    }

    public async Task<Guid> CreateAsync(Guid userId, UpsertBudgetDto dto, CancellationToken cancellationToken = default)
    {
        var budget = new Budget(userId, dto.CategoryId, dto.Year, dto.Month, dto.PlannedAmount);
        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync(cancellationToken);
        return budget.Id;
    }

    public async Task<bool> UpdateAsync(Guid userId, Guid id, UpsertBudgetDto dto, CancellationToken cancellationToken = default)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (budget is null) return false;
        budget.Update(dto.CategoryId, dto.Year, dto.Month, dto.PlannedAmount);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (budget is null) return false;
        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
