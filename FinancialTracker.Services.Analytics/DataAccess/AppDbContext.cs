using FinancialTracker.Services.Analytics.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Expenses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Expense>().HasKey(e => e.ExpenseId);
        modelBuilder.Entity<Expense>().HasIndex(e => new { e.UserId, e.ExpenseId });

        modelBuilder.Entity<User>()
            .HasMany(i => i.Incomes)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Income>().HasKey(i => i.IncomeId);
        modelBuilder.Entity<Income>().HasIndex(i => new {i.UserId, i.IncomeId});
    }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<User> Users { get; set; }
}