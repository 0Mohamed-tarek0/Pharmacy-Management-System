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
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.CompanyName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Address)
                .HasMaxLength(300);

            builder.Property(s => s.Phone)
                .HasMaxLength(20);

            builder.Property(s => s.Email)
                .HasMaxLength(150);
        }
    }
}
