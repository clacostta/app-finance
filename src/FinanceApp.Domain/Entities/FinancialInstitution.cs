using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class FinancialInstitution : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Code { get; private set; }

    public ICollection<Account> Accounts { get; private set; } = new List<Account>();
    public ICollection<CreditCard> CreditCards { get; private set; } = new List<CreditCard>();

    private FinancialInstitution() { }

    public FinancialInstitution(string name, string? code = null)
    {
        Name = name;
        Code = code;
    }
}
