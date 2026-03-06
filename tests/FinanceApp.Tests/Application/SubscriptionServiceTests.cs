using FinanceApp.Application.Services.Subscriptions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FinanceApp.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Tests.Application;

public class SubscriptionServiceTests
{
    [Fact]
    public async Task RefreshDetectionsAsync_ShouldCreateDetections_FromRecurringCandidates()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);
        var userId = Guid.NewGuid();

        var t1 = new Transaction(userId, TransactionType.Expense, "NETFLIX", 39.9m, new DateTime(2025,2,5));
        t1.UpdateManual(TransactionType.Expense, "NETFLIX", 39.9m, new DateTime(2025,2,5), null, null, null, null, true);
        var t2 = new Transaction(userId, TransactionType.Expense, "NETFLIX", 39.9m, new DateTime(2025,3,5));
        t2.UpdateManual(TransactionType.Expense, "NETFLIX", 39.9m, new DateTime(2025,3,5), null, null, null, null, true);
        db.Transactions.AddRange(t1, t2);
        await db.SaveChangesAsync();

        var service = new SubscriptionService(db);
        var created = await service.RefreshDetectionsAsync(userId);

        created.Should().Be(1);
    }
}
