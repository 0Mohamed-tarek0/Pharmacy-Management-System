using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyBL.DTOs.Returns;

namespace PharmacyBL.Interfaces.Services
{
    public interface IReturnService
    {
        /// <summary>All recorded returns - both to suppliers (from purchase Orders)
        /// and from customers (from Sales) - newest first.</summary>
        Task<IEnumerable<ReturnRecordDto>> GetAllAsync();
    }
}
