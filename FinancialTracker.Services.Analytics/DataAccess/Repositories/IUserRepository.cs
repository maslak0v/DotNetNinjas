using FinancialTracker.Services.Analytics.Models;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
    }
}
