namespace PharmacyDAL.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public decimal Amount { get; set; }

        public int ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; } = null!;

        public DateTime ExpenseDate { get; set; }

        public string? Notes { get; set; }

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
