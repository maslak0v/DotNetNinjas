using FinancialTracker.Services.Analytics.Models;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public class ExpensesRepository(AppDbContext db) : IExpensesRepository
{ 
    public IEnumerable<Expense> GetExpenses(Guid userId, DateTime startDate, DateTime endDate)
    {
        var query = db.Set<Expense>();
        return query
            .Where(x => x.ExpenseTime >= startDate.ToUniversalTime()
                        && x.ExpenseTime <= endDate.ToUniversalTime());
    }
}