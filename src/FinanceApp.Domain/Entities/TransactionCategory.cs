using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class TransactionCategory : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsSystemDefault { get; private set; }

    public ICollection<TransactionSubcategory> Subcategories { get; private set; } = new List<TransactionSubcategory>();

    private TransactionCategory() { }

    public TransactionCategory(Guid userId, string name, bool isSystemDefault = false)
    {
        UserId = userId;
        Name = name;
        IsSystemDefault = isSystemDefault;
    }
}
