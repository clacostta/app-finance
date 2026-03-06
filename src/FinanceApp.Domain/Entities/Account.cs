using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class Account : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid FinancialInstitutionId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal InitialBalance { get; private set; }
    public bool IsActive { get; private set; } = true;

    public User User { get; private set; } = default!;
    public FinancialInstitution FinancialInstitution { get; private set; } = default!;
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    private Account() { }

    public Account(Guid userId, Guid financialInstitutionId, string name, decimal initialBalance)
    {
        UserId = userId;
        FinancialInstitutionId = financialInstitutionId;
        Name = name;
        InitialBalance = initialBalance;
    }
}
