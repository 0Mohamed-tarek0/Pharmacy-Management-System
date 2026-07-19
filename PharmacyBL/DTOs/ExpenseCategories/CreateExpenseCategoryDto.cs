using System.ComponentModel.DataAnnotations;

namespace PharmacyBL.DTOs.ExpenseCategories
{
    public class CreateExpenseCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
