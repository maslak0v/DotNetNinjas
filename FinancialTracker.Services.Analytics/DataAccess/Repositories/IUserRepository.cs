using FinancialTracker.Services.Analytics.Models;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
