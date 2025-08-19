using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public class IncomesRepository(AppDbContext db) : IIncomesRepository
{
    public async Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var query = db.Set<Income>().AsNoTracking();
        return await query
            .Where(x => x.UserId == userId &&
                        x.IncomeTime >= startDate.ToUniversalTime() &&
                        x.IncomeTime <= endDate.ToUniversalTime())
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetSumOfIncomesUpToDateAsync(Guid userId, DateTime upToDate, CancellationToken cancellationToken)
    {
        var query = db.Incomes;
        return await query
            .Where(x => x.UserId == userId &&
                        x.IncomeTime <= upToDate.ToUniversalTime())
            .SumAsync(i => i.Amount, cancellationToken);
    }

    public async Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDTO request, CancellationToken cancellationToken)
    {
        var query = db.Set<Income>().AsNoTracking();
        return await query
            .Where(x => x.UserId == request.UserId &&
                        x.AccountId == request.AccountId &&
                        x.IncomeTime >= request.StartDate.ToUniversalTime() &&
                        x.IncomeTime <= request.EndDate.ToUniversalTime())
            .ToListAsync(cancellationToken);
    }
    
    public async Task AddAsync(Income income, CancellationToken cancellationToken)
    {
        db.Incomes.Add(income);
        await db.SaveChangesAsync(cancellationToken);
    }
}
