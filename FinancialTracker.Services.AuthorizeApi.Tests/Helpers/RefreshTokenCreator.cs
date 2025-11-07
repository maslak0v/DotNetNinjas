
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    internal static class RefreshTokenCreator
    {
        internal static RefreshToken Create(string userId, string token)
            => new RefreshToken()
            {
                Jti = Guid.NewGuid(),
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                UserId = userId,
                Token = token
            };
    }
}
