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

    public async Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDto request, CancellationToken cancellationToken)
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

    public async Task<List<CategoryAggregate>> GetIncomeTotalByCategoryAsync
        (Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var startUtc = startDate.Kind == DateTimeKind.Unspecified
        ? DateTime.SpecifyKind(startDate, DateTimeKind.Utc)
        : startDate.ToUniversalTime();

        var endUtc = endDate.Kind == DateTimeKind.Unspecified
        ? DateTime.SpecifyKind(endDate, DateTimeKind.Utc)
        : endDate.ToUniversalTime();

        return await db.Incomes
            .AsNoTracking()
            .Where(e => e.UserId == userId &&
                    e.IncomeTime >= startUtc &&
                    e.IncomeTime <= endUtc)
        .GroupBy(e => e.Category)
        .Select(g => new CategoryAggregate(
            g.Key ?? "Без категории",
            g.Sum(x => x.Amount),
            g.Count()))
        .ToListAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(Guid incomeId, CancellationToken cancellationToken)
    {
        await db.Incomes
            .Where(i => i.IncomeId == incomeId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Guid incomeId,
        string? category,
        string currency,
        decimal amount,
        DateTime incomeTime,
        CancellationToken cancellationToken)
    {
        await db.Incomes
        .Where(e => e.IncomeId == incomeId)
        .ExecuteUpdateAsync(
            e => e
                .SetProperty(p => p.Category, category)
                .SetProperty(p => p.Currency, currency)
                .SetProperty(p => p.Amount, amount)
                .SetProperty(p => p.IncomeTime, incomeTime),
            cancellationToken);
    }
}
