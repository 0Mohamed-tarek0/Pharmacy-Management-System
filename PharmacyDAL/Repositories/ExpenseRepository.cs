using Microsoft.EntityFrameworkCore;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Models;

namespace PharmacyDAL.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Expense>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(e => e.ExpenseCategory)
                .Include(e => e.ApplicationUser)
                .OrderByDescending(e => e.ExpenseDate)
                .ThenByDescending(e => e.Id)
                .ToListAsync();
        }

        public async Task<Expense?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(e => e.ExpenseCategory)
                .Include(e => e.ApplicationUser)
                .SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}
