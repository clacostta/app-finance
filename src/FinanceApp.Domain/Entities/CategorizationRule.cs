using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class CategorizationRule : BaseEntity
{
    public Guid UserId { get; private set; }
    public string FieldName { get; private set; } = string.Empty;
    public string Operator { get; private set; } = "contains";
    public string CompareValue { get; private set; } = string.Empty;
    public Guid CategoryId { get; private set; }
    public Guid? SubcategoryId { get; private set; }
    public int Priority { get; private set; } = 100;
    public bool IsActive { get; private set; } = true;

    private CategorizationRule() { }

    public CategorizationRule(Guid userId, string fieldName, string compareValue, Guid categoryId)
    {
        UserId = userId;
        FieldName = fieldName;
        CompareValue = compareValue;
        CategoryId = categoryId;
    }
}
