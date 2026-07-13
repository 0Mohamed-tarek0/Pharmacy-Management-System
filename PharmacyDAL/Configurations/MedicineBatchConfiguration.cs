using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyDAL.Models;

namespace PharmacyDAL.Configurations
{
    public class MedicineBatchConfiguration : IEntityTypeConfiguration<MedicineBatch>
    {
        public void Configure(EntityTypeBuilder<MedicineBatch> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.BatchNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.SellingPrice)
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(b => new { b.MedicineId, b.BatchNumber })
                    .IsUnique();

            // The same batch number cannot be used twice for the same medicine.
            builder.HasOne(b => b.Medicine)
                   .WithMany(m => m.Batches)
                   .HasForeignKey(b => b.MedicineId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Supplier)
                .WithMany(s => s.Batches)
                .HasForeignKey(b => b.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
