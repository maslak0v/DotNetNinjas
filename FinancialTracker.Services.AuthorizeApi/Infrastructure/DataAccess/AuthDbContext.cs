using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options)
        : IdentityDbContext<AuthUser>(options)
    { }
}
