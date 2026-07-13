using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDAL.Models
{
    public class Supplier
    {

        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        // Navigation properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<MedicineSupplier> MedicineSuppliers { get; set; } = new List<MedicineSupplier>();
        public ICollection<MedicineBatch> Batches { get; set; } = new List<MedicineBatch>();
    }
}
