using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Web.Models.Budgets;

public class BudgetFormViewModel
{
    public Guid? Id { get; set; }
    [Required] public Guid CategoryId { get; set; }
    [Range(2000, 2100)] public int Year { get; set; } = DateTime.Today.Year;
    [Range(1, 12)] public int Month { get; set; } = DateTime.Today.Month;
    [Range(0.01, 999999999)] public decimal PlannedAmount { get; set; }
}
