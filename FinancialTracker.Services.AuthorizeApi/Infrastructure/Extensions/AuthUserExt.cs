using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Extensions
{
    public static class AuthUserExt
    {
        public  static bool CheckRole(this AuthUser user, string role)
            => user.Roles
                .Select(r => r.Name)
                .Any(r => r!.Equals(role, StringComparison.OrdinalIgnoreCase));
    }
}
