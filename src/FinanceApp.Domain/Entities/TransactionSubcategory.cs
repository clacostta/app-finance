using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class TransactionSubcategory : BaseEntity
{
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public TransactionCategory Category { get; private set; } = default!;

    private TransactionSubcategory() { }

    public TransactionSubcategory(Guid categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }
}
