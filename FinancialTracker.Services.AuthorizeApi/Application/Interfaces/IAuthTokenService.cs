
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
        Task<RefreshToken> GenerateRefreshTokenAsync(string userId, CancellationToken cancellationToken);
        string GenerateAccessToken(User user, string jti);
        Task<RefreshToken?> FindRefreshTokenByJtiAsync(Guid jti, CancellationToken cancellationToken);
        Task Revoke(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task RevokeAllForUserAsync(string userId, CancellationToken cancellationToken);
        Task<bool> IsRevokedRefreshTokenAsync(string jti, CancellationToken cancellationToken);
    }
}
