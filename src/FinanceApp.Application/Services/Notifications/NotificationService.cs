using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Notifications;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IAppDbContext _context;

    public NotificationService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<NotificationDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(100)
            .Select(x => new NotificationDto(x.Id, x.Title, x.Message, x.IsRead, x.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GenerateBudgetAlertsAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken = default)
    {
        var start = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        var end = start.AddMonths(1);

        var budgets = await _context.Budgets
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Year == referenceDate.Year && x.Month == referenceDate.Month)
            .ToListAsync(cancellationToken);

        var spendByCategory = await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Type == TransactionType.Expense && x.TransactionDate >= start && x.TransactionDate < end && x.CategoryId.HasValue)
            .GroupBy(x => x.CategoryId!.Value)
            .Select(g => new { CategoryId = g.Key, Total = g.Sum(x => x.Amount) })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Total, cancellationToken);

        var generated = 0;
        foreach (var b in budgets)
        {
            var spent = spendByCategory.TryGetValue(b.CategoryId, out var total) ? total : 0m;
            if (spent <= b.PlannedAmount) continue;

            var existing = await _context.Notifications.AnyAsync(x =>
                x.UserId == userId && x.Title == "Orçamento excedido" && x.Message.Contains(b.CategoryId.ToString()), cancellationToken);
            if (existing) continue;

            _context.Notifications.Add(new Notification(userId, "Orçamento excedido", $"Categoria {b.CategoryId} excedeu o orçamento planejado. Planejado: {b.PlannedAmount:C}, realizado: {spent:C}."));
            generated++;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return generated;
    }

    public async Task<bool> MarkAsReadAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var n = await _context.Notifications.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (n is null) return false;
        n.MarkAsRead();
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
