using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyDAL.Enums;

namespace PharmacyBL.DTOs.Medicines
{
    /// <summary>Row for the Medicine list screen - aggregated across all batches.</summary>
    public class MedicineDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MedicineType Type { get; set; }

        public string Barcode { get; set; }

        public string CategoryName { get; set; }

        public string? ImagePath { get; set; }

        /// <summary>Sum of all batch quantities, in the base unit.</summary>
        public int TotalQuantity { get; set; }

        public int MinimumStock { get; set; }

        ///// <summary>Lowest selling price across batches with stock (what a pharmacist would quote first, FEFO).</summary>
        //public decimal? SellingPrice { get; set; }

        public bool IsLowStock => TotalQuantity <= MinimumStock;

        public decimal? SellingPrice { get; internal set; }

        public List<MedicineUnitDto> Units { get; set; } = new();
    }
}
