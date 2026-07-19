namespace PharmacyBL.DTOs.Expenses
{
    public class ExpenseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public decimal Amount { get; set; }

        public int ExpenseCategoryId { get; set; }

        public string ExpenseCategoryName { get; set; } = null!;

        public DateTime ExpenseDate { get; set; }

        public string? Notes { get; set; }

        public string ApplicationUserId { get; set; } = null!;

        public string RecordedBy { get; set; } = null!;
    }
}
