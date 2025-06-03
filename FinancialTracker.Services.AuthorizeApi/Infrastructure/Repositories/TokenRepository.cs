using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Repositories
{
    public class TokenRepository(AuthDbContext dbcontext) : ITokenRepository
    {
        public void Add(RefreshToken token)
        {
            var tokenModel = token.EntityModelFromDomain();
            dbcontext.RefreshTokens.Add(tokenModel);
        }
        public async Task<RefreshToken?> FindAsync(string jti)
        {
            var refreshTokenModel =  await dbcontext.RefreshTokens.FindAsync(jti);
            return refreshTokenModel?.ToRefreshTokenDomain();
        }

        public void Revoke(string jti)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync() => await dbcontext.SaveChangesAsync();
    }
}
