using FinanceApp.Application.Abstractions;
using FinanceApp.Application.DTOs;
using FinanceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services;

public class DashboardService
{
    private readonly IAppDbContext _context;

    public DashboardService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken = default)
    {
        var monthStart = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        var income = await _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Income && t.TransactionDate >= monthStart && t.TransactionDate < monthEnd)
            .SumAsync(t => (decimal?)t.Amount, cancellationToken) ?? 0m;

        var expense = await _context.Transactions
            .Where(t => t.UserId == userId && t.Type == TransactionType.Expense && t.TransactionDate >= monthStart && t.TransactionDate < monthEnd)
            .SumAsync(t => (decimal?)t.Amount, cancellationToken) ?? 0m;

        var balance = income - expense;

        return new DashboardSummaryDto(balance, income, expense, income - expense);
    }
}
