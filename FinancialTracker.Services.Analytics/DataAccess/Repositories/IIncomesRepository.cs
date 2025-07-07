using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public interface IIncomesRepository
{
    /// <summary>
    /// Получить список доходов за период для пользователя
    /// </summary>
    /// <param name="userId">Guid пользователя</param>
    /// <param name="startDate">Дата начала периода</param>
    /// <param name="endDate">Дата окончания периода</param>
    /// <returns></returns>
    Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate);
    
    /// <summary>
    /// Получить список доходов за период для пользователя с учетом счета и валюты
    /// </summary>
    /// <param name="request">Параметры запроса</param>
    /// <returns>список доходов</returns>
    Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDTO request);
    
    /// <summary>
    /// Получить список доходов до указанной даты включительно
    /// </summary>
    /// <param name="userId">Guid пользователя</param>
    /// <param name="upToDate">Дата, до которой получаем доходы (включительно)</param>
    /// <returns>Список доходов</returns>
    Task<List<Income>> GetIncomesUpToDateAsync(Guid userId, DateTime upToDate);
}
