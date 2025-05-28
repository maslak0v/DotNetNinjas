namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class BalanceService(IExpensesService expensesService) : IBalanceService
{
    public async Task<decimal> GetBalanceAsync(Guid userId, DateTime forDate)
    {
        var startBalance = 0;
        
        var expenses = await expensesService.GetExpensesBeforeDateAsync(userId, forDate);
        var sumOfExpenses = expenses.Sum(e => e.Amount);
        
        return (startBalance - sumOfExpenses);
    }
}