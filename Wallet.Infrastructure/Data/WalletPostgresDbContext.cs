using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data;

public class WalletPostgresDbContext : DbContext
{
    public DbSet<Account> Account { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public WalletPostgresDbContext(DbContextOptions<WalletPostgresDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
          modelBuilder.Entity<Account>(entity =>
    {
        entity.HasKey(a => a.AccountId);

        entity.Property(a => a.Name)
              .IsRequired();
              
        entity.Property(a => a.CurrentBalance)
              .HasColumnType("decimal(18,2)");
              
        entity.Property(a => a.Currency)
              .HasConversion<string>();
              
        entity.HasIndex(a => a.UserId);
        
        entity.HasMany(a => a.Transactions)
              .WithOne(t => t.Account)
              .HasForeignKey(t => t.AccountId)
              .OnDelete(DeleteBehavior.Cascade);
        
        entity.Property(a => a.CreatedAt)
              .HasDefaultValueSql("NOW()");
    });
    
    // Конфигурация Transaction
    modelBuilder.Entity<Transaction>(entity =>
    {
        entity.HasKey(t => t.TransactionId);
        
        entity.Property(t => t.Amount)
              .HasColumnType("decimal(18,2)");
              
        entity.Property(t => t.Comment)
              .HasMaxLength(1000);
              
        entity.Property(t => t.Image)
              .HasMaxLength(1000);
              
        entity.Property(t => t.OperationType)
              .HasConversion<string>();
              
        entity.HasOne(t => t.Category)
              .WithMany(c => c.Transactions)
              .HasForeignKey(t => t.CategoryId)
              .OnDelete(DeleteBehavior.Restrict);
        
        entity.HasOne(t => t.Tag)
              .WithMany(t => t.Transactions)
              .HasForeignKey(t => t.TagId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull);
        
        entity.HasIndex(t => t.AccountId);
        entity.HasIndex(t => t.CategoryId);
        entity.HasIndex(t => t.TagId);
        
        entity.Property(t => t.TransactionDate)
              .HasDefaultValueSql("NOW()");
        
        entity.Property(t => t.UpdatedAt)
              .IsRequired(false)
              .ValueGeneratedOnAddOrUpdate();
    });

    modelBuilder.Entity<Category>(entity =>
    {
          entity.HasKey(c => c.CategoryId);
    
          entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);
          
          entity.Property(c => c.Icon)
                .HasMaxLength(100)
                .HasDefaultValue("default");
          
          entity.HasIndex(c => c.Name)
                .IsUnique();
    
          entity.Property(c => c.CreatedAt)
                .HasDefaultValueSql("NOW()");
    });
    
    modelBuilder.Entity<Tag>(entity =>
    {
        entity.HasKey(t => t.TagId);
        
        entity.Property(t => t.Name)
              .IsRequired()
              .HasMaxLength(250);
              
        entity.HasIndex(t => new { t.Name, t.UserId }) 
              .IsUnique();
              
        entity.HasIndex(t => t.UserId);
    });
    }
}