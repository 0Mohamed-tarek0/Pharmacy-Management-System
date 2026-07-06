using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryWithMedicinesAsync(int categoryId);

        Task<IEnumerable<Category>> GetAllWithMedicineCountAsync();
    }
}
