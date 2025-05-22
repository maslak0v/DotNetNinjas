using Microsoft.AspNetCore.Identity;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Interfaces
{
    public interface ITokenService<TUser> where TUser : IdentityUser
    {
        Task<string> GenerateTokenAsync(TUser user);
        string GenerateRefreshToken();
    }
}
