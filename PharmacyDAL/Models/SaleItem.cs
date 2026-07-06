using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDAL.Models
{
    public class SaleItem
    {

        public int Id { get; set; }

        // Foreign Key -> Sale
        public int SaleId { get; set; }
        public Sale Sale { get; set; }

        // Foreign Key -> Medicine
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }
}
