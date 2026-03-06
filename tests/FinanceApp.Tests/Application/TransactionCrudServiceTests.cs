using FinanceApp.Application.Services.Transactions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FinanceApp.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Tests.Application;

public class TransactionCrudServiceTests
{
    [Fact]
    public async Task ListAsync_ShouldApplyPaginationAndSearch()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);
        var userId = Guid.NewGuid();

        for (var i = 0; i < 40; i++)
        {
            db.Transactions.Add(new Transaction(userId, TransactionType.Expense, i % 2 == 0 ? $"Mercado {i}" : $"Uber {i}", 10 + i, new DateTime(2025, 3, 1).AddDays(i % 20)));
        }

        await db.SaveChangesAsync();

        var service = new TransactionCrudService(db);

        var total = await service.CountAsync(userId, null, null, "Mercado");
        var page1 = await service.ListAsync(userId, null, null, "Mercado", 1, 10);
        var page2 = await service.ListAsync(userId, null, null, "Mercado", 2, 10);

        total.Should().Be(20);
        page1.Should().HaveCount(10);
        page2.Should().HaveCount(10);
        page1.All(x => x.Description.Contains("Mercado")).Should().BeTrue();
    }
}
