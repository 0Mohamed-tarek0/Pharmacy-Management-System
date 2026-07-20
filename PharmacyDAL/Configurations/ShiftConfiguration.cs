using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyDAL.Models;

namespace PharmacyDAL.Configurations
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.OpeningCash).HasColumnType("decimal(18,2)");
            builder.Property(s => s.SalesTotal).HasColumnType("decimal(18,2)");
            builder.Property(s => s.ReturnsTotal).HasColumnType("decimal(18,2)");
            builder.Property(s => s.ExpectedCash).HasColumnType("decimal(18,2)");
            builder.Property(s => s.ActualCash).HasColumnType("decimal(18,2)");
            builder.Property(s => s.CashDifference).HasColumnType("decimal(18,2)");

            // SQL Server filtered index: one cashier can have only one active shift.
            builder.HasIndex(s => s.ApplicationUserId)
                .IsUnique()
                .HasFilter("[ClosedAt] IS NULL");

            builder.HasOne(s => s.ApplicationUser)
                .WithMany(u => u.Shifts)
                .HasForeignKey(s => s.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
