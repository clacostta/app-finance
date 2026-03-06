using FinanceApp.Application.Services.Dashboard;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FinanceApp.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Tests.Application;

public class DashboardAnalyticsServiceTests
{
    [Fact]
    public async Task GetAsync_ShouldCalculateSummary()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);
        var userId = Guid.NewGuid();
        var now = new DateTime(2025, 3, 12);

        db.Transactions.Add(new Transaction(userId, TransactionType.Income, "Salário", 5000m, new DateTime(2025, 3, 5)));
        db.Transactions.Add(new Transaction(userId, TransactionType.Expense, "Aluguel", 2000m, new DateTime(2025, 3, 6)));
        await db.SaveChangesAsync();

        var service = new DashboardAnalyticsService(db);
        var result = await service.GetAsync(userId, now);

        result.Summary.MonthlyIncome.Should().Be(5000m);
        result.Summary.MonthlyExpense.Should().Be(2000m);
        result.Summary.MonthlySavings.Should().Be(3000m);
    }
}
