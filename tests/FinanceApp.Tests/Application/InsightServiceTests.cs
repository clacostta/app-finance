using FinanceApp.Application.Services.Insights;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FinanceApp.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Tests.Application;

public class InsightServiceTests
{
    [Fact]
    public async Task GetInsightsAsync_ShouldReturnVariationInsight_WhenExpenseChangesOver10Percent()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);

        var userId = Guid.NewGuid();
        var now = new DateTime(2025, 3, 10);

        db.Transactions.Add(new Transaction(userId, TransactionType.Expense, "Mercado", 100m, new DateTime(2025, 2, 10)));
        db.Transactions.Add(new Transaction(userId, TransactionType.Expense, "Mercado", 130m, new DateTime(2025, 3, 5)));
        await db.SaveChangesAsync();

        var service = new InsightService(db);
        var result = await service.GetInsightsAsync(userId, now);

        result.Any(x => x.Title.Contains("Variação")).Should().BeTrue();
    }
}
