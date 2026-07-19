using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmacyDAL.Models;

namespace PharmacyDAL.Configurations
{
    public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
    {
        public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasMany(c => c.Expenses)
                .WithOne(e => e.ExpenseCategory)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new ExpenseCategory { Id = 1, Name = "Rent" },
                new ExpenseCategory { Id = 2, Name = "Electricity" },
                new ExpenseCategory { Id = 3, Name = "Internet" },
                new ExpenseCategory { Id = 4, Name = "Water" },
                new ExpenseCategory { Id = 5, Name = "Salaries" },
                new ExpenseCategory { Id = 6, Name = "Maintenance" },
                new ExpenseCategory { Id = 7, Name = "Transportation" },
                new ExpenseCategory { Id = 8, Name = "Other" }
            );
        }
    }
}
