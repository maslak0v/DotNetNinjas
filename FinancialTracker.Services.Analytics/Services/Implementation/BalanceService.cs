namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class BalanceService(IExpensesService expensesService) : IBalanceService
{
    public decimal GetBalance(Guid userId, DateTime forDate)
    {
        var startBalance = 0;
        var sumOfExpenses = expensesService.GetExpensesBeforeDate(userId, forDate)
            .Sum(e => e.Amount);
        return (startBalance - sumOfExpenses);
    }
}