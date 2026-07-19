using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface IExpenseCategoryRepository : IGenericRepository<ExpenseCategory>
    {
        Task<ExpenseCategory?> GetCategoryWithExpensesAsync(int categoryId);
    }
}
