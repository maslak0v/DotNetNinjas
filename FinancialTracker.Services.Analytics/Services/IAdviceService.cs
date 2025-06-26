using FinancialTracker.Services.Analytics.Models.Advice;

namespace FinancialTracker.Services.Analytics.Services;

public interface IAdviceService
{
    /// <summary>
    /// Получить список рекомендаций для пользователя за период
    /// </summary>
    /// <param name="userId"> Guid пользователя </param>
    /// <param name="startDate"> Дата начала периода </param>
    /// <param name="endDate">Дата окончания периода </param>
    /// <returns></returns>
    Task<List<AdviceResult>> GetAdviceAsync(Guid userId, DateTime startDate, DateTime endDate);
}