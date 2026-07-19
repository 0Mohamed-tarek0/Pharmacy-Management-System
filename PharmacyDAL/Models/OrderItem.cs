using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDAL.Models
{
    /// <summary>
    /// One line of a purchase Order. Carries the batch information supplied
    /// by the pharmacist (batch number, expiry, prices, unit) so the
    /// OrderService can create/update the matching MedicineBatch when the
    /// order is received.
    /// </summary>
    public class OrderItem
    {
        public int Id { get; set; }

        // Foreign Key -> Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Foreign Key -> Medicine
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        /// <summary>Quantity as entered by the user, in <see cref="UnitName"/> units.</summary>
        public int Quantity { get; set; }

        /// <summary>Unit the quantity was entered in (e.g. "Box", "Strip"). Defaults to the base unit.</summary>
        public string UnitName { get; set; } = string.Empty;

        public decimal PurchasePrice { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }

        public string BatchNumber { get; set; } = string.Empty;

        public DateTime ExpiryDate { get; set; }

        /// <summary>Resale price to store on the batch once received.</summary>
        public decimal SellingPrice { get; set; }

        // Foreign Key -> MedicineBatch (set once the order has been received / stock created)
        public int? MedicineBatchId { get; set; }
        public MedicineBatch? MedicineBatch { get; set; }

        /// <summary>
        /// How much of <see cref="Quantity"/> (in <see cref="UnitName"/> units) has already
        /// been returned to the supplier. Stock for a return is taken back out of
        /// <see cref="MedicineBatch"/> and can never exceed <see cref="Quantity"/>.
        /// </summary>
        public int ReturnedQuantity { get; set; } = 0;
    }
}
