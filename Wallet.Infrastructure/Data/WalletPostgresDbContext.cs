using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data;

public class WalletPostgresDbContext : DbContext
{
    public DbSet<Account> Account { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TransactionTag> TransactionTags { get; set; }

    public WalletPostgresDbContext(DbContextOptions<WalletPostgresDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(w => w.AccountId);
            
            entity.Property(w => w.Name)
                  .IsRequired()
                  .HasMaxLength(100);
                  
            entity.Property(w => w.CurrentBalance)
                  .HasColumnType("decimal(18,2)");
                  
            entity.Property(w => w.Currency)
                  .HasConversion<string>(); //  enum как строку 
                  
            entity.HasIndex(w => w.UserId);
            
            entity.HasMany(w => w.Transactions)
                  .WithOne(t => t.Account)
                  .HasForeignKey(t => t.WalletId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.TransactionId);
            
            entity.Property(t => t.Amount)
                  .HasColumnType("decimal(18,2)");
                  
            entity.Property(t => t.Comment)
                  .HasMaxLength(500);
                  
            entity.Property(t => t.Image)
                  .HasMaxLength(255);
                  
            entity.Property(t => t.OperationType)
                  .HasConversion<string>();
                  
            entity.HasOne(t => t.Category)
                  .WithMany(c => c.Transactions)
                  .HasForeignKey(t => t.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasIndex(t => t.WalletId);
            entity.HasIndex(t => t.CategoryId);
        });

        // Конфигурация Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.CategoryId);
            
            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(50);
                  
            entity.Property(c => c.Icon)
                  .HasMaxLength(50)
                  .HasDefaultValue("default");
            
            entity.HasIndex(c => c.Name)
                  .IsUnique();
        });

        // Конфигурация Tag
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(t => t.TagId);
            
            entity.Property(t => t.Name)
                  .IsRequired()
                  .HasMaxLength(50);
                  
            entity.HasIndex(t => t.UserId);
        });
        
        modelBuilder.Entity<TransactionTag>(entity =>
        {
            entity.HasKey(tt => new { tt.TransactionId, tt.TagId });
            
            entity.HasOne(tt => tt.Transaction)
                  .WithMany(t => t.TransactionTags)
                  .HasForeignKey(tt => tt.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(tt => tt.Tag)
                  .WithMany(t => t.TransactionTags)
                  .HasForeignKey(tt => tt.TagId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}