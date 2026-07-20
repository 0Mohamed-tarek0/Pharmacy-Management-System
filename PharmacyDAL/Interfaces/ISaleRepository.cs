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

        Task<IEnumerable<Sale>> GetAllWithDetailsAsync();

        /// <summary>
        /// Returns the SUM of <see cref="Sale.TotalAmount"/> for completed sales
        /// whose <see cref="Sale.InvoiceDate"/> falls within [from, to].
        /// </summary>
        Task<decimal> GetTotalByDateRangeAsync(DateTime from, DateTime to);

        Task<decimal> GetTotalByUserAndDateRangeAsync(string applicationUserId, DateTime from, DateTime to);
    }
}
