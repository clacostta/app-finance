using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Web.Models.CreditCards;

public class CreditCardFormViewModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 999999999)]
    public decimal LimitAmount { get; set; }

    [Range(1, 31)]
    public int ClosingDay { get; set; }

    [Range(1, 31)]
    public int DueDay { get; set; }

    [Required]
    public Guid FinancialInstitutionId { get; set; }

    public bool IsActive { get; set; } = true;
}
