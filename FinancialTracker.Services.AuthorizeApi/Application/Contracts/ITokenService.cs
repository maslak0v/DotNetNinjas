using Microsoft.AspNetCore.Identity;

namespace FinancialTracker.Services.AuthorizeApi.Application.Contracts
{
    public interface ITokenService<TUser> where TUser : IdentityUser
    {
        Task<string> GenerateTokenAsync(TUser user);
        string GenerateRefreshToken();
    }
}
