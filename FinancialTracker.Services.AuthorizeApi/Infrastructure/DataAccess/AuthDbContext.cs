using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options)
        : IdentityDbContext<AuthUser, AuthRole, string>(options)
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

            builder.Entity<AuthUser>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<IdentityUserRole<string>>(
                    l => l.HasOne<AuthRole>().WithMany().HasForeignKey(e => e.RoleId),
                    r => r.HasOne<AuthUser>().WithMany().HasForeignKey(e => e.UserId));

            builder.Entity<RefreshTokenModel>()
                .HasIndex(r => r.ExpiresAt);
        }

    }
}
