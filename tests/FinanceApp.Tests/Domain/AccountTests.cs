using FinanceApp.Domain.Entities;
using FluentAssertions;

namespace FinanceApp.Tests.Domain;

public class AccountTests
{
    [Fact]
    public void Update_ShouldChangeEditableFields()
    {
        var account = new Account(Guid.NewGuid(), Guid.NewGuid(), "Conta A", 100m);
        var newInstitution = Guid.NewGuid();

        account.Update("Conta B", 250m, false, newInstitution);

        account.Name.Should().Be("Conta B");
        account.InitialBalance.Should().Be(250m);
        account.IsActive.Should().BeFalse();
        account.FinancialInstitutionId.Should().Be(newInstitution);
    }
}
