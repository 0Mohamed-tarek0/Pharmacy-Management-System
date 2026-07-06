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
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        public SaleRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Sale> GetSaleWithItemsAsync(int saleId)
        {
            return await _dbSet
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Medicine)
                .Include(s => s.ApplicationUser)
                .SingleOrDefaultAsync(s => s.Id == saleId);
        }

        public async Task<Sale> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _dbSet
                .Include(s => s.SaleItems)
                .SingleOrDefaultAsync(s => s.InvoiceNumber == invoiceNumber);
        }

        public async Task<IEnumerable<Sale>> GetSalesByUserAsync(string applicationUserId)
        {
            return await _dbSet
                .Where(s => s.ApplicationUserId == applicationUserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _dbSet
                .Where(s => s.InvoiceDate >= from && s.InvoiceDate <= to)
                .ToListAsync();
        }
    }
}
