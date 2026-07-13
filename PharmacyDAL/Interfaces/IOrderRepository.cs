using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Models;

namespace PharmacyDAL.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrderWithItemsAsync(int orderId);

        Task<Order> GetByOrderNumberAsync(string orderNumber);

        Task<IEnumerable<Order>> GetOrdersBySupplierAsync(int supplierId);

        Task<IEnumerable<Order>> GetOrdersByUserAsync(string applicationUserId);

        Task<IEnumerable<Order>> GetAllWithDetailsAsync();

        //Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
    }
}
