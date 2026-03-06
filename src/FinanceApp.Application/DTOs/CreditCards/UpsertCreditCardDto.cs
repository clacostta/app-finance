namespace FinanceApp.Application.DTOs.CreditCards;

public class UpsertCreditCardDto
{
    public string Name { get; set; } = string.Empty;
    public decimal LimitAmount { get; set; }
    public int ClosingDay { get; set; }
    public int DueDay { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid FinancialInstitutionId { get; set; }
}
