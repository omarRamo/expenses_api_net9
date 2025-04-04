using Expenses.Domain.Entities;
using Expenses.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Infrastructure.DbContext;

public class ExpensesDbContext(DbContextOptions<ExpensesDbContext> options) 
    : Microsoft.EntityFrameworkCore.DbContext(options)
{

    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<ExpenseType> ExpenseTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpensesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
        modelBuilder.ApplyConfiguration(new ExpenseTypeConfiguration());

        // Seed data

        var fixedDate = new DateTime(2025, 3, 20);

        modelBuilder.Entity<Currency>().HasData(
            new Currency { IdCurrency = 1, CurrencyCode = "USD", Name = "U.S Dollar", CreatedDate = fixedDate },
            new Currency { IdCurrency = 2, CurrencyCode = "RUB", Name = "Russian Ruble", CreatedDate = fixedDate }
        );

        modelBuilder.Entity<ExpenseType>().HasData(
            new ExpenseType { IdTypeExpenses = 1, Label = "Restaurant", CreatedDate = fixedDate },
            new ExpenseType { IdTypeExpenses = 2, Label = "Hotel", CreatedDate = fixedDate },
            new ExpenseType { IdTypeExpenses = 3, Label = "Misc", CreatedDate = fixedDate }
        );

        modelBuilder.Entity<User>().HasData(
            new User { IdUser = 1, FirstName = "Anthony", LastName = "Stark", CreatedDate = fixedDate, CurrencyId = 1 },
            new User { IdUser = 2, FirstName = "Natasha", LastName = "Romanova", CreatedDate = fixedDate, CurrencyId = 2 }
        );
    }
}
