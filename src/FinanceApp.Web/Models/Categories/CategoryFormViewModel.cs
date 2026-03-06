using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Web.Models.Categories;

public class CategoryFormViewModel
{
    public Guid? Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;
}
