using System.ComponentModel.DataAnnotations;

namespace Pharmacy.ViewModels.ExpenseCategories
{
    public class CreateExpenseCategoryViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = null!;
    }
}
