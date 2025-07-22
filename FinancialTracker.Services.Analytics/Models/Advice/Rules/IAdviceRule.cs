namespace FinancialTracker.Services.Analytics.Models.Advice.Rules;

public interface IAdviceRule
{
    Task<AdviceResult> EvaluateAsync(List<Expense> expenses, CancellationToken cancellationToken);
}