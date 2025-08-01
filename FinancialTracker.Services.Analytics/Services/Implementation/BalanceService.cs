namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class BalanceService(
    IExpensesService expensesService,
    IIncomesService incomesService) : IBalanceService
{
    public async Task<decimal> GetBalanceAsync(Guid userId, DateTime forDate, CancellationToken cancellationToken)
    {
        var startBalance = 0;
        
        var expenses = await expensesService.GetExpensesUpToDateAsync(userId, forDate, cancellationToken);
        var incomes = await incomesService.GetIncomesUpToDateAsync(userId, forDate, cancellationToken);

        var sumOfExpenses = expenses.Sum(e => e.Amount);
        var sumOfIncomes = incomes.Sum(i => i.Amount);

        return (startBalance + sumOfIncomes - sumOfExpenses);
    }
}