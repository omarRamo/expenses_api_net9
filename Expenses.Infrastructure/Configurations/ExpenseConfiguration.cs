using Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Infrastructure.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.IdExpense);
        builder.Property(e => e.Amount).HasPrecision(18, 2);
        builder.HasOne(e => e.User)
               .WithMany(u => u.Expenses)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Currency)
               .WithMany(c => c.Expenses)
               .HasForeignKey(e => e.CurrencyId);
        builder.HasOne(e => e.ExpenseType)
               .WithMany(et => et.Expenses)
               .HasForeignKey(e => e.ExpenseTypesId);
    }
}
