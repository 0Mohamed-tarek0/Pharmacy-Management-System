using System.ComponentModel.DataAnnotations;

namespace PharmacyBL.DTOs.Expenses
{
    public class UpdateExpenseDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        public int ExpenseCategoryId { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
