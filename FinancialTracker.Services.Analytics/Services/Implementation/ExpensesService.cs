using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class ExpensesService (IExpensesRepository expensesRepository): IExpensesService
{
    public async Task<List<Expense>> GetExpensesAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await expensesRepository.GetExpensesAsync(userId, startDate, endDate);
    }
    
    public async Task<List<Expense>> GetExpensesUpToDateAsync(Guid userId, DateTime upToDate)
    {
        return await expensesRepository.GetExpensesUpToDateAsync(userId, upToDate);
    }

    public async Task<List<Expense>> GetExpensesByAccountAsync(ExpensesRequestDto request)
    {
        return await expensesRepository.GetExpensesByAccountAsync(request);
    }
}