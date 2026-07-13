using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyBL.DTOs.Orders
{
    public class OrderItemViewDto
    {
        public string MedicineName { get; set; }

        public string UnitName { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal Discount { get; set; }

        public string BatchNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public decimal Total { get; set; }
    }
}
