using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyDAL.Models;

namespace PharmacyDAL.Configurations
{
    public class MedicineUnitConfiguration : IEntityTypeConfiguration<MedicineUnit>
    {
        public void Configure(EntityTypeBuilder<MedicineUnit> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.UnitName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ConversionFactor)
                   .IsRequired();

            builder.HasIndex(u => new { u.MedicineId, u.UnitName })
                .IsUnique();

            builder.HasOne(u => u.Medicine)
                .WithMany(m => m.Units)
                .HasForeignKey(u => u.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
