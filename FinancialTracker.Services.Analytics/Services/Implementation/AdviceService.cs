using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models.Advice;
using FinancialTracker.Services.Analytics.Models.Advice.Rules;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class AdviceService(IEnumerable<IAdviceRule> rules, IExpensesRepository expensesRepository) : IAdviceService
{ 
    public async Task<List<AdviceResult>> GetAdviceAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var expenses = await expensesRepository.GetExpensesAsync(userId, startDate, endDate, cancellationToken);
        var results = await Task.WhenAll(
            rules.Select(r => r.EvaluateAsync(expenses, cancellationToken)));

        return results.ToList();
    }
}