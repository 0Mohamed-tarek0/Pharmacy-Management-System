using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyDAL.Models;

namespace PharmacyDAL.Configurations
{
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.ReferenceType)
                .HasMaxLength(30);

            builder.Property(t => t.Notes)
                .HasMaxLength(300);

            builder.HasOne(t => t.Medicine)
                .WithMany(m => m.StockTransactions)
                .HasForeignKey(t => t.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.MedicineBatch)
                .WithMany(b => b.StockTransactions)
                .HasForeignKey(t => t.MedicineBatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
