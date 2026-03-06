using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Subscriptions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly IAppDbContext _context;

    public SubscriptionService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<SubscriptionDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.SubscriptionDetections
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.AverageAmount)
            .Select(x => new SubscriptionDto(x.Id, x.Merchant, x.AverageAmount, x.OccurrenceCount, x.AverageAmount * 12, x.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> RefreshDetectionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var candidates = await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Type == TransactionType.Expense && x.IsSubscriptionCandidate)
            .GroupBy(x => x.Description)
            .Select(g => new { Merchant = g.Key, Count = g.Count(), Avg = g.Average(x => x.Amount) })
            .Where(x => x.Count >= 2)
            .ToListAsync(cancellationToken);

        var created = 0;
        foreach (var c in candidates)
        {
            var exists = await _context.SubscriptionDetections.AnyAsync(x => x.UserId == userId && x.Merchant == c.Merchant && x.IsActive, cancellationToken);
            if (exists) continue;
            _context.SubscriptionDetections.Add(new SubscriptionDetection(userId, c.Merchant, (decimal)c.Avg, c.Count));
            created++;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return created;
    }

    public async Task<bool> DeactivateAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var item = await _context.SubscriptionDetections.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (item is null) return false;
        item.Deactivate();
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
