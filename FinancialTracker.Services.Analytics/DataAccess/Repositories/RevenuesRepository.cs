using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories
{
    public class RevenuesRepository(AppDbContext db) : IRevenuesRepository
    {
        public async Task<List<Revenue>> GetRevenuesAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var query = db.Set<Revenue>().AsNoTracking();
            return await query
            .Where(x => x.User.Guid == userId &&
                       x.RevenueTime >= startDate.ToUniversalTime()
                        && x.RevenueTime <= endDate.ToUniversalTime())
            .ToListAsync();
        }

        public async Task<List<Revenue>> GetRevenuesBeforeDateAsync(Guid userId, DateTime date)
        {
            var query = db.Set<Revenue>().AsNoTracking();
            return await query
                .Where(x => x.User.Guid == userId &&
                            x.RevenueTime <= date.ToUniversalTime())
                .ToListAsync();
        }

        public async Task<List<Revenue>> GetRevenuesByAccountAsync(RevenuesRequestDto request)
        {
            var query = db.Set<Revenue>().AsNoTracking();
            return await query
                .Where(x => x.User.Guid == request.UserId &&
                x.AccountId == request.AccountId &&
                x.RevenueTime >= request.StartDate.ToUniversalTime() &&
                x.RevenueTime <= request.EndDate.ToUniversalTime())
                .ToListAsync();
        }
    }
}
