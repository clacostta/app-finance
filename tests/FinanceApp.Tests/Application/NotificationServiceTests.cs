using FinanceApp.Application.Services.Notifications;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FinanceApp.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Tests.Application;

public class NotificationServiceTests
{
    [Fact]
    public async Task GenerateBudgetAlertsAsync_ShouldCreateNotification_WhenSpentExceedsPlan()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);
        var userId = Guid.NewGuid();
        var category = new TransactionCategory(userId, "Alimentação");
        db.TransactionCategories.Add(category);
        db.Budgets.Add(new Budget(userId, category.Id, 2025, 3, 100m));
        var tx = new Transaction(userId, TransactionType.Expense, "Mercado", 150m, new DateTime(2025,3,5));
        tx.UpdateManual(TransactionType.Expense, "Mercado", 150m, new DateTime(2025,3,5), null, null, category.Id, null, false);
        db.Transactions.Add(tx);
        await db.SaveChangesAsync();

        var service = new NotificationService(db);
        var created = await service.GenerateBudgetAlertsAsync(userId, new DateTime(2025,3,10));

        created.Should().Be(1);
    }
}
