using FinancialTracker.Services.AuthorizeApi.Domain.Entities;

namespace FinancialTracker.Services.AuthorizeApi.Application.Interfaces
{
    public interface ITokenRepository
    {
        Task SaveAsync();
        void Add(RefreshToken token);
        Task<RefreshToken?> FindByJtiAsync(Guid jti);
        Task RevokeAsync(RefreshToken refreshToken);
        Task RevokeAllForUserAsync(string userId);
    }
}
