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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.Discount)
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.Total)
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.SellingPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.BatchNumber)
                .HasMaxLength(50);

            builder.Property(oi => oi.UnitName)
                .HasMaxLength(50);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Medicine)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(oi => oi.MedicineBatch)
                .WithMany()
                .HasForeignKey(oi => oi.MedicineBatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
