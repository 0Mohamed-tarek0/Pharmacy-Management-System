using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetAllWithDetailsAsync();

        Task<Expense?> GetByIdWithDetailsAsync(int id);

        /// <summary>
        /// Returns the SUM of <see cref="Expense.Amount"/> for expenses
        /// whose <see cref="Expense.ExpenseDate"/> falls within [from, to].
        /// </summary>
        Task<decimal> GetTotalByDateRangeAsync(DateTime from, DateTime to);
    }
}
