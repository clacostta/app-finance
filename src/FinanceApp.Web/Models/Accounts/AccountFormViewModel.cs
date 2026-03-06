using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Web.Models.Accounts;

public class AccountFormViewModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 999999999)]
    public decimal InitialBalance { get; set; }

    [Required]
    public Guid FinancialInstitutionId { get; set; }

    public bool IsActive { get; set; } = true;
}
