namespace FinanceApp.Domain.Enums;

public enum ImportBatchStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    CompletedWithWarnings = 4,
    Failed = 5
}
