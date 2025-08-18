using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public class ExpensesRepository(AppDbContext db) : IExpensesRepository
{
    public async Task<List<Expense>> GetExpensesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return await query
            .Where(x => x.UserId == userId &&
                       x.ExpenseTime >= startDate.ToUniversalTime()
                        && x.ExpenseTime <= endDate.ToUniversalTime())
            .ToListAsync(cancellationToken);
    }
    
    public async Task<decimal> GetSumOfExpensesUpToDateAsync(Guid userId, DateTime upToDate, CancellationToken cancellationToken)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return await query
            .Where(x => x.UserId == userId &&
                        x.ExpenseTime <= upToDate.ToUniversalTime())
            .SumAsync(e => e.Amount, cancellationToken);
    }

    public async Task<List<Expense>> GetExpensesByAccountAsync(ExpensesRequestDto request, CancellationToken cancellationToken)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return await query
            .Where(x => x.UserId == request.UserId &&
                       x.AccountId == request.AccountId &&
                       x.ExpenseTime >= request.StartDate.ToUniversalTime() &&
                       x.ExpenseTime <= request.EndDate.ToUniversalTime())
            .ToListAsync(cancellationToken);
    }
    
    public async Task AddAsync(Expense expense, CancellationToken cancellationToken)
    {
        db.Expenses.Add(expense);
        await db.SaveChangesAsync(cancellationToken);
    }
}