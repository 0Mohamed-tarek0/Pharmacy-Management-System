using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        Task<Sale> GetSaleWithItemsAsync(int saleId);

        Task<Sale> GetByInvoiceNumberAsync(string invoiceNumber);

        Task<IEnumerable<Sale>> GetSalesByUserAsync(string applicationUserId);

        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime from, DateTime to);
    }
}
