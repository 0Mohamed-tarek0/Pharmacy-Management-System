using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyDAL.Interfaces;
using PharmacyDAL.Models;

namespace PharmacyDAL.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category> GetCategoryWithMedicinesAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.Medicines)
                .SingleOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<IEnumerable<Category>> GetAllWithMedicineCountAsync()
        {
            return await _dbSet
                .Include(c => c.Medicines)
                .ToListAsync();
        }
    }
}
