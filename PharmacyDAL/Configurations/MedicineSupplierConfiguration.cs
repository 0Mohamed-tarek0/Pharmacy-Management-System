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
    public class MedicineSupplierConfiguration : IEntityTypeConfiguration<MedicineSupplier>
    {
        public void Configure(EntityTypeBuilder<MedicineSupplier> builder)
        {
            builder.HasKey(ms => ms.Id);

            builder.Property(ms => ms.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(ms => ms.Medicine)
                .WithMany(m => m.MedicineSuppliers)
                .HasForeignKey(ms => ms.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ms => ms.Supplier)
                .WithMany(s => s.MedicineSuppliers)
                .HasForeignKey(ms => ms.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
