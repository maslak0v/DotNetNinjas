using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class ExpensesService (IExpensesRepository expensesRepository, IMapper mapper): IExpensesService
{
    public async Task<List<Expense>> GetExpensesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return await expensesRepository.GetExpensesAsync(userId, startDate, endDate, cancellationToken);
    }
    
    public async Task<decimal> GetSumOfExpensesUpToDateAsync(Guid userId, DateTime upToDate, CancellationToken cancellationToken)
    {
        return await expensesRepository.GetSumOfExpensesUpToDateAsync(userId, upToDate, cancellationToken);
    }

    public async Task<List<Expense>> GetExpensesByAccountAsync(ExpensesRequestDto request, CancellationToken cancellationToken)
    {
        return await expensesRepository.GetExpensesByAccountAsync(request, cancellationToken);
    }

    public async Task AddAsync(ExpenseDto expenseDto, CancellationToken cancellationToken)
    {
        var expense = mapper.Map<Expense>(expenseDto);
        await expensesRepository.AddAsync(expense, cancellationToken);
    }
}