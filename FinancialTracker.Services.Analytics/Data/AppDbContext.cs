using FinancialTracker.Services.Analytics.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var userDasha = new User()
        {
            Name = "Даша",
            UserId = 1
        };

        modelBuilder.Entity<User>().HasData(userDasha);
    }
}