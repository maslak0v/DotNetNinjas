using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public class ExpensesRepository(AppDbContext db) : IExpensesRepository
{
    public IEnumerable<Expense> GetExpenses(Guid userId, DateTime startDate, DateTime endDate)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return query
            .Where(x => x.User.Guid == userId &&
                       x.ExpenseTime >= startDate.ToUniversalTime()
                        && x.ExpenseTime <= endDate.ToUniversalTime());
    }

    public IEnumerable<Expense> GetExpensesByAccount(ExpensesRequestDto request)
    {
        var query = db.Set<Expense>().AsNoTracking();
        return query
            .Where(x => x.User.Guid == request.UserId &&
                       x.AccountId == request.AccountId &&
                       x.ExpenseTime >= request.StartDate.ToUniversalTime() &&
                       x.ExpenseTime <= request.EndDate.ToUniversalTime());
    }
}