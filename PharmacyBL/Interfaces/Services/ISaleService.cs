using System.Threading.Tasks;
using PharmacyBL.DTOs.Sales;

namespace PharmacyBL.Interfaces.Services
{
    public interface ISaleService
    {
        /// <summary>
        /// Creates a Sale and deducts stock FEFO (First-Expire-First-Out):
        /// for every line, the requested quantity (converted to the base
        /// unit) is taken from the batch with the nearest expiry date first,
        /// then the next, and so on until the line is fully covered.
        /// </summary>
        Task<int> CreateAsync(CreateSaleDto dto);
    }
}
