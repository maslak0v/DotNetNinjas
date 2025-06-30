using FinancialTracker.Services.Analytics.Models;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        public async Task AddAsync(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
