using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDAL.Models
{
    public class MedicineSupplier
    {

        public int Id { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public decimal PurchasePrice { get; set; }
    }
}
