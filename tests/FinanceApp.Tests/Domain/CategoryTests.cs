using FinanceApp.Domain.Entities;
using FluentAssertions;

namespace FinanceApp.Tests.Domain;

public class CategoryTests
{
    [Fact]
    public void Rename_ShouldUpdateName()
    {
        var category = new TransactionCategory(Guid.NewGuid(), "Mercado");

        category.Rename("Supermercado");

        category.Name.Should().Be("Supermercado");
    }
}
