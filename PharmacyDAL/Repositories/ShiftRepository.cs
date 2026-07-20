using Microsoft.EntityFrameworkCore;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Models;

namespace PharmacyDAL.Repositories
{
    public class ShiftRepository : GenericRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(DbContext.ApplicationDbContext context) : base(context) { }

        public Task<Shift?> GetOpenByUserAsync(string applicationUserId) =>
            _dbSet.Include(s => s.ApplicationUser)
                .SingleOrDefaultAsync(s => s.ApplicationUserId == applicationUserId && s.ClosedAt == null);

        public Task<Shift?> GetByIdWithUserAsync(int id) =>
            _dbSet.Include(s => s.ApplicationUser).SingleOrDefaultAsync(s => s.Id == id);

        public async Task<IEnumerable<Shift>> GetClosedByUserAsync(string applicationUserId) =>
            await _dbSet.Include(s => s.ApplicationUser)
                .Where(s => s.ApplicationUserId == applicationUserId && s.ClosedAt != null)
                .OrderByDescending(s => s.ClosedAt)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<Shift>> GetAllClosedAsync() =>
            await _dbSet.Include(s => s.ApplicationUser)
                .Where(s => s.ClosedAt != null)
                .OrderByDescending(s => s.ClosedAt)
                .AsNoTracking()
                .ToListAsync();
    }
}
