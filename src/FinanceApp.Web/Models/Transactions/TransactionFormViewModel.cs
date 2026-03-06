using System.ComponentModel.DataAnnotations;
using FinanceApp.Domain.Enums;

namespace FinanceApp.Web.Models.Transactions;

public class TransactionFormViewModel
{
    public Guid? Id { get; set; }

    [Required]
    public DateTime TransactionDate { get; set; } = DateTime.Today;

    [Required, StringLength(250)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 999999999)]
    public decimal Amount { get; set; }

    [Required]
    public TransactionType Type { get; set; }

    public Guid? AccountId { get; set; }
    public Guid? CreditCardId { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Notes { get; set; }
    public bool IsRecurring { get; set; }
}
