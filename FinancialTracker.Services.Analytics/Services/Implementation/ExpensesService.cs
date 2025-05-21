using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class ExpensesService (IExpensesRepository expensesRepository): IExpensesService
{
    public IEnumerable<Expense> GetExpenses(Guid userId, DateTime startDate, DateTime endDate)
    {
        return expensesRepository.GetExpenses(userId, startDate, endDate);
    }
    
    public IEnumerable<Expense> GetExpensesBeforeDate(Guid userId, DateTime beforeDate)
    {
        return expensesRepository.GetExpensesBeforeDate(userId, beforeDate);
    }

    public IEnumerable<Expense> GetExpensesByAccount(ExpensesRequestDto request)
    {
        if (request.StartDate > request.EndDate)
        {
            throw new ArgumentException("Дата начала периода не может быть позже даты окончания");
        }

        return expensesRepository.GetExpensesByAccount(request);
    }
}