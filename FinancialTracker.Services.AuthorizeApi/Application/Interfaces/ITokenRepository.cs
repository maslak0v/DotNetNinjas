using FinancialTracker.Services.AuthorizeApi.Domain.Entities;

namespace FinancialTracker.Services.AuthorizeApi.Application.Interfaces
{
    public interface ITokenRepository
    {
        Task SaveAsync(CancellationToken cancellationToken);
        void Add(RefreshToken token);
        Task<RefreshToken?> FindByJtiAsync(Guid jti, CancellationToken cancellationToken);
        Task RevokeAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task RevokeAllForUserAsync(string userId, CancellationToken cancellationToken);
        Task RevokeAllAsync(CancellationToken cancellationToken);
    }
}
