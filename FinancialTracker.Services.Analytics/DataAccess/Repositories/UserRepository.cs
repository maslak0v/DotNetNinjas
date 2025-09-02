using FinancialTracker.Services.Analytics.Models;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(userId, cancellationToken);
            dbContext.Users.Remove(user!);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
