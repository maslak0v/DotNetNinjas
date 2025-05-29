using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping
{
    public static class MappingRefreshToken
    {
        public static RefreshTokenModel EntityModelFromDomain(this RefreshToken rt)
            => new(rt.Token, rt.UserId, rt.Jti, rt.ExpiresAt);

        public static RefreshToken ToRefreshTokenDomain(this RefreshTokenModel rtm) =>
            RefreshToken.CopyData(rtm.Jti, rtm.Token, rtm.UserId, rtm.ExpiresAt, rtm.IsRevoked);
    }
}
