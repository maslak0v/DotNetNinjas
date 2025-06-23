using FinancialTracker.Services.Analytics.Models.Advice;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class AdviceService : IAdviceService
{
    public Task<List<AdviceResult>> GetAdviceAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        var resultList = new List<AdviceResult>
        {
            new AdviceResult
            {
                Message = "Отлично, что фиксируешь траты!",
                Title = "Учёт трат"
            }
        };

        return Task.FromResult(resultList);
    }
}