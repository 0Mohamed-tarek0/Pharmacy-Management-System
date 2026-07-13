using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Enums;

namespace PharmacyDAL.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; }

        // Foreign Key -> Supplier
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public DateTime OrderDate { get; set; }

        // Foreign Key -> ApplicationUser
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public OrderStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

        // Navigation property
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
