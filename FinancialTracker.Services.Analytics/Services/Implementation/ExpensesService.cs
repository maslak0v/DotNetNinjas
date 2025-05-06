using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class ExpensesService (IExpensesRepository expensesRepository): IExpensesService
{
    public IEnumerable<Expense> GetExpenses(Guid userId, DateTime startDate, DateTime endDate)
    {
        return expensesRepository.GetExpenses(userId, startDate, endDate);
    }
}