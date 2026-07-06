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
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(s => s.InvoiceNumber)
                .IsUnique();

            builder.Property(s => s.Status)
                .HasMaxLength(30);

            builder.Property(s => s.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(s => s.ApplicationUser)
                .WithMany(u => u.Sales)
                .HasForeignKey(s => s.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
