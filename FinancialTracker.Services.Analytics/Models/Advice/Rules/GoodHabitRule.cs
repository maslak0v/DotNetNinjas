namespace FinancialTracker.Services.Analytics.Models.Advice.Rules;

public class GoodHabitRule : IAdviceRule
{
    public Task<AdviceResult> EvaluateAsync(List<Expense> expenses)
    {
        var result = expenses.Count != 0
            ? new AdviceResult
            {
                Title = "Отлично, что фиксируешь траты!",
                Message = "Учёт трат"
            }
            : new AdviceResult();

        return Task.FromResult(result);
    }
}