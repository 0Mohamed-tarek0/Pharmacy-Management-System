using System;
using System.Collections.Generic;
using PharmacyDAL.Enums;

namespace PharmacyBL.DTOs.Medicines
{
    /// <summary>
    /// Everything shown on the "Medicine Details" screen: the medicine as one
    /// entity, plus every batch (with its own price/expiry/qty) and the units
    /// it can be bought/sold in.
    /// </summary>
    public class MedicineDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MedicineType Type { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string CategoryName { get; set; }
        public string? ImagePath { get; set; }
        public int MinimumStock { get; set; }

        public int TotalQuantity { get; set; }

        public List<MedicineBatchDto> Batches { get; set; } = new();
        public List<MedicineUnitDto> Units { get; set; } = new();
    }
}
