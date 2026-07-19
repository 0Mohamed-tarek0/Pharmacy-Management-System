using Microsoft.EntityFrameworkCore;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Models;

namespace PharmacyDAL.Repositories
{
    public class ExpenseCategoryRepository : GenericRepository<ExpenseCategory>, IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ExpenseCategory?> GetCategoryWithExpensesAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Expenses)
                .SingleOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}
