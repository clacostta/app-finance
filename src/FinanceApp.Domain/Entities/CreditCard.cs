using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class CreditCard : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid FinancialInstitutionId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal LimitAmount { get; private set; }
    public int ClosingDay { get; private set; }
    public int DueDay { get; private set; }
    public bool IsActive { get; private set; } = true;

    public User User { get; private set; } = default!;
    public FinancialInstitution FinancialInstitution { get; private set; } = default!;

    private CreditCard() { }

    public CreditCard(Guid userId, Guid institutionId, string name, decimal limitAmount, int closingDay, int dueDay)
    {
        UserId = userId;
        FinancialInstitutionId = institutionId;
        Name = name;
        LimitAmount = limitAmount;
        ClosingDay = closingDay;
        DueDay = dueDay;
    }
}
