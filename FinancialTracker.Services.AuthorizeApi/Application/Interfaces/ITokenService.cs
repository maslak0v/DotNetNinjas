
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;

namespace FinancialTracker.Services.AuthorizeApi.Application.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Create and save to db
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<RefreshToken> GenerateRefreshTokenAsync(string userId);
        string GenerateAccessToken(User user, string jti, IList<string> roles);
    }
}
