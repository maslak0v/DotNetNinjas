using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public class ExpensesRepository(AppDbContext db) : IExpensesRepository
{
    public async Task<List<Expense>> GetExpensesAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return await query
            .Where(x => x.User.Id == userId &&
                       x.ExpenseTime >= startDate.ToUniversalTime()
                        && x.ExpenseTime <= endDate.ToUniversalTime())
            .ToListAsync();
    }
    
    public async Task<List<Expense>> GetExpensesBeforeDateAsync(Guid userId, DateTime date)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return await query
            .Where(x => x.User.Id == userId &&
                        x.ExpenseTime <= date.ToUniversalTime())
            .ToListAsync();
    }

    public async Task<List<Expense>> GetExpensesByAccountAsync(ExpensesRequestDto request)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return await query
            .Where(x => x.User.Id == request.UserId &&
                       x.AccountId == request.AccountId &&
                       x.ExpenseTime >= request.StartDate.ToUniversalTime() &&
                       x.ExpenseTime <= request.EndDate.ToUniversalTime())
            .ToListAsync();
    }
}