using System.ComponentModel.DataAnnotations;

namespace PharmacyBL.DTOs.ExpenseCategories
{
    public class UpdateExpenseCategoryDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
