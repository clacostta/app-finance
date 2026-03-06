using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FluentAssertions;

namespace FinanceApp.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void HasSameSignature_ShouldReturnTrue_WhenSignatureMatches()
    {
        var tx = new Transaction(Guid.NewGuid(), TransactionType.Expense, "Mercado Central", 120.50m, new DateTime(2025, 1, 10));

        var same = tx.HasSameSignature(new DateTime(2025, 1, 10), 120.50m, "mercado central");

        same.Should().BeTrue();
    }
}
