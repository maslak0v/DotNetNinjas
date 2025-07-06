
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;

namespace FinancialTracker.Services.AuthorizeApi.Application.Interfaces
{
    public interface IAuthTokenService
    {
        /// <summary>
        /// Create and save to db
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<RefreshToken> GenerateRefreshTokenAsync(string userId);
        string GenerateAccessToken(User user, string jti);
        Task<RefreshToken?> FindRefreshTokenByJtiAsync(Guid jti);
        Task Revoke(RefreshToken refreshToken);
        Task RevokeAllForUserAsync(string userId);
        Task<bool> IsRevokedRefreshTokenAsync(string jti);
    }
}
