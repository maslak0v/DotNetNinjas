using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Repositories
{
    public class TokenRepository(AuthDbContext dbcontext) : ITokenRepository
    {
        public void Add(RefreshToken token)
        {
            var tokenModel = token.EntityModelFromDomain();
            dbcontext.RefreshTokens.Add(tokenModel);
        }
        public async Task<RefreshToken?> FindByJtiAsync(Guid jti)
        {
            var refreshTokenModel =  await dbcontext.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Jti == jti);
            if (refreshTokenModel is null || refreshTokenModel.IsRevoked)
                return null;
            return refreshTokenModel?.ToRefreshTokenDomain();
        }

        public async Task SaveAsync() => await dbcontext.SaveChangesAsync();

        public async Task RevokeAsync(RefreshToken token)
        {
            RefreshTokenModel tokenModel = token.EntityModelFromDomain();
            token.IsRevoked = true;
            dbcontext.RefreshTokens.Update(tokenModel);
            await dbcontext.SaveChangesAsync();
        }

        public async Task RevokeAllForUserAsync(string userId)
        {
            await dbcontext.RefreshTokens
                .Where(t => !t.IsRevoked && userId == t.UserId)
                .ExecuteUpdateAsync(t => t.SetProperty(p => p.IsRevoked, true));
        }
    }
}
