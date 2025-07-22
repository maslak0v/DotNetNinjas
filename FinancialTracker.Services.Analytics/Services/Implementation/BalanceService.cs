namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class BalanceService(IExpensesService expensesService) : IBalanceService
{
    public async Task<decimal> GetBalanceAsync(Guid userId, DateTime forDate, CancellationToken cancellationToken)
    {
        var startBalance = 0;
        
        var expenses = await expensesService.GetExpensesUpToDateAsync(userId, forDate, cancellationToken);
        var sumOfExpenses = expenses.Sum(e => e.Amount);
        
        return (startBalance - sumOfExpenses);
    }
}