using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDAL.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Foreign Key -> Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Foreign Key -> Medicine
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }
}
