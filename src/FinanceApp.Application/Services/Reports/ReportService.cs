using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Reports;
using FinanceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Reports;

public class ReportService : IReportService
{
    private readonly IAppDbContext _context;

    public ReportService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ReportBundleDto> GetBundleAsync(Guid userId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
            .Where(x => x.UserId == userId && x.TransactionDate >= from.Date && x.TransactionDate < to.Date.AddDays(1));

        var expenseByCategory = await query
            .Where(x => x.Type == TransactionType.Expense)
            .GroupBy(x => x.CategoryId)
            .Select(g => new ReportLineDto(g.Key.HasValue ? g.Key.Value.ToString() : "Sem categoria", g.Sum(x => x.Amount)))
            .OrderByDescending(x => x.Total)
            .Take(10)
            .ToListAsync(cancellationToken);

        var incomeByMonth = await query
            .Where(x => x.Type == TransactionType.Income)
            .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month })
            .Select(g => new ReportLineDto($"{g.Key.Month:00}/{g.Key.Year}", g.Sum(x => x.Amount)))
            .OrderBy(x => x.Label)
            .ToListAsync(cancellationToken);

        var expenseByMonth = await query
            .Where(x => x.Type == TransactionType.Expense)
            .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month })
            .Select(g => new ReportLineDto($"{g.Key.Month:00}/{g.Key.Year}", g.Sum(x => x.Amount)))
            .OrderBy(x => x.Label)
            .ToListAsync(cancellationToken);

        var topExpenseDescriptions = await query
            .Where(x => x.Type == TransactionType.Expense)
            .GroupBy(x => x.Description)
            .Select(g => new ReportLineDto(g.Key, g.Sum(x => x.Amount)))
            .OrderByDescending(x => x.Total)
            .Take(10)
            .ToListAsync(cancellationToken);

        return new ReportBundleDto
        {
            ExpenseByCategory = expenseByCategory,
            IncomeByMonth = incomeByMonth,
            ExpenseByMonth = expenseByMonth,
            TopExpenseDescriptions = topExpenseDescriptions
        };
    }
}
