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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext.ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order> GetOrderWithItemsAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Medicine)
                .Include(o => o.Supplier)
                .Include(o => o.ApplicationUser)
                .SingleOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order> GetByOrderNumberAsync(string orderNumber)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .SingleOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<IEnumerable<Order>> GetOrdersBySupplierAsync(int supplierId)
        {
            return await _dbSet
                .Where(o => o.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string applicationUserId)
        {
            return await _dbSet
                .Where(o => o.ApplicationUserId == applicationUserId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(o => o.Supplier)
                .Include(o => o.ApplicationUser)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        //public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        //{
        //    return await _dbSet
        //        .Where(o => o.Status = status)
        //        .ToListAsync();
        //}
    }
}
