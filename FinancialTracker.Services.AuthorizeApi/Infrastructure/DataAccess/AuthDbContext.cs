using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options)
        : IdentityDbContext<AuthUser>(options)
    {
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshTokenModel>()
                .HasKey(r => r.Jti);
                
            builder.Entity<RefreshTokenModel>()
                .HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RefreshToken>()
                .HasIndex(r => r.ExpiresAt);
        }

    }
}
