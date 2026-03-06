using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs;
using FinanceApp.Application.DTOs.Dashboard;
using FinanceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Dashboard;

public class DashboardAnalyticsService : IDashboardAnalyticsService
{
    private readonly IAppDbContext _context;

    public DashboardAnalyticsService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardAnalyticsDto> GetAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken = default)
    {
        var monthStart = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var monthEnd = monthStart.AddMonths(1);
        var prevStart = monthStart.AddMonths(-1);
        var prevEnd = monthStart;

        var tx = _context.Transactions.Where(x => x.UserId == userId);

        var summary = await BuildSummary(tx, monthStart, monthEnd, cancellationToken);
        var prevSummary = await BuildSummary(tx, prevStart, prevEnd, cancellationToken);

        var expenseByCategory = await tx
            .Where(x => x.TransactionDate >= monthStart && x.TransactionDate < monthEnd && x.Type == TransactionType.Expense)
            .GroupBy(x => x.CategoryId)
            .Select(g => new CategoryExpenseDto(g.Key.HasValue ? g.Key.Value.ToString() : "Sem categoria", g.Sum(x => x.Amount)))
            .OrderByDescending(x => x.Total)
            .Take(8)
            .ToListAsync(cancellationToken);

        var monthlyTrend = await tx
            .Where(x => x.TransactionDate >= monthStart.AddMonths(-5) && x.TransactionDate < monthEnd)
            .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month })
            .Select(g => new MonthlyTrendPointDto(
                $"{g.Key.Month:00}/{g.Key.Year}",
                g.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount),
                g.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount),
                g.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount) - g.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount)))
            .OrderBy(x => x.Label)
            .ToListAsync(cancellationToken);

        var cashFlowByDay = await tx
            .Where(x => x.TransactionDate >= monthStart && x.TransactionDate < monthEnd)
            .GroupBy(x => x.TransactionDate.Date)
            .Select(g => new CashFlowPointDto(
                g.Key.ToString("dd/MM"),
                g.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount),
                g.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount)))
            .OrderBy(x => x.Day)
            .ToListAsync(cancellationToken);

        return new DashboardAnalyticsDto
        {
            Summary = summary,
            PreviousSummary = prevSummary,
            ExpenseByCategory = expenseByCategory,
            MonthlyTrend = monthlyTrend,
            CashFlowByDay = cashFlowByDay
        };
    }

    private static async Task<DashboardSummaryDto> BuildSummary(IQueryable<Domain.Entities.Transaction> tx, DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        var income = await tx.Where(x => x.Type == TransactionType.Income && x.TransactionDate >= start && x.TransactionDate < end)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0;
        var expense = await tx.Where(x => x.Type == TransactionType.Expense && x.TransactionDate >= start && x.TransactionDate < end)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0;

        return new DashboardSummaryDto(income - expense, income, expense, income - expense);
    }
}
