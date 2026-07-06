using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyDAL.Models;

namespace PharmacyDAL.Configurations
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.HasKey(si => si.Id);

            builder.Property(si => si.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(si => si.Discount)
                .HasColumnType("decimal(18,2)");

            builder.Property(si => si.Total)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(si => si.Sale)
                .WithMany(s => s.SaleItems)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(si => si.Medicine)
                .WithMany(m => m.SaleItems)
                .HasForeignKey(si => si.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
