using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Web.Models.Goals;

public class GoalFormViewModel
{
    public Guid? Id { get; set; }
    [Required, StringLength(120)] public string Name { get; set; } = string.Empty;
    [Range(0.01, 999999999)] public decimal TargetAmount { get; set; }
    [Range(0, 999999999)] public decimal CurrentAmount { get; set; }
    public DateTime? TargetDate { get; set; }
}
