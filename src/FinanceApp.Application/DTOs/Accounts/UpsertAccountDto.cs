namespace FinanceApp.Application.DTOs.Accounts;

public class UpsertAccountDto
{
    public string Name { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid FinancialInstitutionId { get; set; }
}
