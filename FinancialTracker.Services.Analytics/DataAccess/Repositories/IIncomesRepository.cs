using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public interface IIncomesRepository: IUnitOfWork
{
    /// <summary>
    /// Получить список доходов за период для пользователя
    /// </summary>
    /// <param name="userId">Guid пользователя</param>
    /// <param name="startDate">Дата начала периода</param>
    /// <param name="endDate">Дата окончания периода</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

    /// <summary>
    /// Получить список доходов за период для пользователя с учетом счета и валюты
    /// </summary>
    /// <param name="request">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>список доходов</returns>
    Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDto request, CancellationToken cancellationToken);

    /// <summary>
    /// Получить сумму доходов до указанной даты включительно
    /// </summary>
    /// <param name="userId">Guid пользователя</param>
    /// <param name="upToDate">Дата, до которой получаем доходы (включительно)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список доходов</returns>
    Task<decimal> GetSumOfIncomesUpToDateAsync(Guid userId, DateTime upToDate, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить новый доход
    /// </summary>
    /// <param name="income">Доход</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task AddAsync(Income income, CancellationToken cancellationToken);

    /// <summary>
    /// Получить доходы по категориям
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<CategoryAggregate>> GetIncomeTotalByCategoryAsync(
        Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить доход по ID
    /// </summary>
    /// <param name="incomeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteByIdAsync(Guid incomeId, CancellationToken cancellationToken);

    /// <summary>
    /// Изменить доход
    /// </summary>
    /// <param name="incomeId"></param>
    /// <param name="category"></param>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    /// <param name="incomeTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(
        Guid incomeId,
        string? category,
        string currency,
        decimal amount,
        DateTime incomeTime,
        CancellationToken cancellationToken);
}
