namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class BalanceService(
    IExpensesService expensesService,
    IIncomesService incomesService) : IBalanceService
{
    public async Task<decimal> GetBalanceAsync(Guid userId, DateTime forDate, CancellationToken cancellationToken)
    {
        var startBalance = 0;
        
        var sumOfExpenses = await expensesService.GetSumOfExpensesUpToDateAsync(userId, forDate, cancellationToken);
        var sumOfIncomes = await incomesService.GetSumOfIncomesUpToDateAsync(userId, forDate, cancellationToken);

        return (startBalance + sumOfIncomes - sumOfExpenses);
    }
}