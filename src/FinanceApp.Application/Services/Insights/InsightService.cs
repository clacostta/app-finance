using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Insights;
using FinanceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Insights;

public class InsightService : IInsightService
{
    private readonly IAppDbContext _context;

    public InsightService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<InsightDto>> GetInsightsAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken = default)
    {
        var monthStart = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var prevStart = monthStart.AddMonths(-1);
        var monthEnd = monthStart.AddMonths(1);
        var prevEnd = monthStart;

        var thisMonthExpense = await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Type == TransactionType.Expense && x.TransactionDate >= monthStart && x.TransactionDate < monthEnd)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0;

        var prevMonthExpense = await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Type == TransactionType.Expense && x.TransactionDate >= prevStart && x.TransactionDate < prevEnd)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0;

        var insights = new List<InsightDto>();

        if (prevMonthExpense > 0)
        {
            var variation = ((thisMonthExpense - prevMonthExpense) / prevMonthExpense) * 100;
            if (Math.Abs(variation) >= 10)
            {
                insights.Add(new InsightDto(
                    "Variação de despesas",
                    $"Suas despesas variaram {variation:N1}% em relação ao mês anterior.",
                    variation > 0 ? "warning" : "info"));
            }
        }

        var topOutlier = await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Type == TransactionType.Expense && x.TransactionDate >= monthStart && x.TransactionDate < monthEnd)
            .OrderByDescending(x => x.Amount)
            .Select(x => new { x.Description, x.Amount })
            .FirstOrDefaultAsync(cancellationToken);

        if (topOutlier is not null && topOutlier.Amount > 1000)
        {
            insights.Add(new InsightDto(
                "Gasto fora do padrão",
                $"Detectamos um gasto elevado: {topOutlier.Description} ({topOutlier.Amount:C}).",
                "danger"));
        }

        var recurringCandidates = await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.IsSubscriptionCandidate)
            .CountAsync(cancellationToken);

        if (recurringCandidates > 0)
        {
            insights.Add(new InsightDto(
                "Possíveis assinaturas",
                $"Encontramos {recurringCandidates} lançamentos com perfil de assinatura. Avalie no painel de assinaturas.",
                "info"));
        }

        if (insights.Count == 0)
        {
            insights.Add(new InsightDto("Sem alertas críticos", "Seu comportamento financeiro está estável no período atual.", "success"));
        }

        return insights;
    }
}
