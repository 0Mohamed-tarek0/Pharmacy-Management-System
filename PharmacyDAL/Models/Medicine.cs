using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDAL.Models
{
    public class Medicine
    {


        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal PurchasePrice { get; set; }

        public int StockQuantity { get; set; }

        public int MinimumStock { get; set; }

        public string ImagePath { get; set; }

        public string Barcode { get; set; }

        public DateTime ManufactureDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        // Foreign Key -> Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation properties
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<MedicineSupplier> MedicineSuppliers { get; set; } = new List<MedicineSupplier>();
    }
}
