namespace PharmacyBL.DTOs.Medicines
{
    public class MedicineUnitDto
    {
        public int Id { get; set; }
        public string UnitName { get; set; }
        public int ConversionFactor { get; set; }
        public bool IsBaseUnit { get; set; }
    }
}
