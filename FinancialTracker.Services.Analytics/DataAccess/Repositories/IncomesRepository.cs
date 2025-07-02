using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public class IncomesRepository(AppDbContext db) : IIncomesRepository
{
    public async Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        var query = db.Set<Income>().AsNoTracking();
        return await query
            .Where(x => x.UserId == userId &&
                        x.IncomeTime >= startDate.ToUniversalTime() &&
                        x.IncomeTime <= endDate.ToUniversalTime())
            .ToListAsync();
    }

    public async Task<List<Income>> GetIncomesBeforeDateAsync(Guid userId, DateTime date)
    {
        var query = db.Set<Income>().AsNoTracking();
        return await query
            .Where(x => x.UserId == userId &&
                        x.IncomeTime <= date.ToUniversalTime())
            .ToListAsync();
    }

    public async Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDTO request)
    {
        var query = db.Set<Income>().AsNoTracking();
        return await query
            .Where(x => x.UserId == request.UserId &&
                        x.AccountId == request.AccountId &&
                        x.IncomeTime >= request.StartDate.ToUniversalTime() &&
                        x.IncomeTime <= request.EndDate.ToUniversalTime())
            .ToListAsync();
    }
}
