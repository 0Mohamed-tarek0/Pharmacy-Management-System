using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Enums;

namespace PharmacyDAL.Models
{
    /// <summary>
    /// Represents the medicine itself (its identity), independent of any
    /// particular batch/lot. Price, expiry date and quantity now live on
    /// <see cref="MedicineBatch"/>, since the same medicine can have several
    /// batches with different prices/expiry dates/quantities.
    /// </summary>
    public class Medicine
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MedicineType Type { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Reorder threshold, expressed in the base unit, compared against the
        /// sum of all batch quantities.
        /// </summary>
        public int MinimumStock { get; set; }

        public string? ImagePath { get; set; }

        public string Barcode { get; set; }

        // Foreign Key -> Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation properties
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<MedicineSupplier> MedicineSuppliers { get; set; } = new List<MedicineSupplier>();
        public ICollection<MedicineBatch> Batches { get; set; } = new List<MedicineBatch>();
        public ICollection<MedicineUnit> Units { get; set; } = new List<MedicineUnit>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}
