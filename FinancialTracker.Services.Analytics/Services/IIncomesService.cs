using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services
{
    public interface IIncomesService
    {
        /// <summary>
        /// Получить список доходов за период для пользователя
        /// </summary>
        /// <param name="userId"> Guid пользователя </param>
        /// <param name="startDate"> Дата начала периода </param>
        /// <param name="endDate">Дата окончания периода </param>
        /// <returns></returns>
        Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Получить список доходов до указанной даты (включительно) для пользователя
        /// </summary>
        /// <param name="userId"> Guid пользователя </param>
        /// <param name="beforeDate"> Дата, до которой получаем доходы </param>
        /// <returns> Список доходов </returns>
        Task<List<Income>> GetIncomesBeforeDateAsync(Guid userId, DateTime beforeDate);

        /// <summary>
        /// Получить список доходов за период для пользователя с учетом счета и валюты
        /// </summary>
        /// <param name="request">Параметры запроса</param>
        /// <returns>Список доходов</returns>
        Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDto request);
    }
}
