using System;

namespace PharmacyDAL.Models
{
    /// <summary>
    /// Defines a sellable/purchasable unit for a Medicine and how many
    /// base units it is equal to. Example (Panadol): Box -> 10, Strip -> 1.
    /// The unit with ConversionFactor == 1 is the Medicine's base unit,
    /// and is what stock is always stored/deducted in.
    /// </summary>
    public class MedicineUnit
    {
        public int Id { get; set; }

        // Foreign Key -> Medicine
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public string UnitName { get; set; }

        /// <summary>
        /// How many base units make up one of this unit.
        /// The base unit itself is stored with ConversionFactor = 1.
        /// </summary>
        public int ConversionFactor { get; set; }

        public bool IsBaseUnit { get; set; }
    }
}
