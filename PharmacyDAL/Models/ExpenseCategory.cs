namespace PharmacyDAL.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
