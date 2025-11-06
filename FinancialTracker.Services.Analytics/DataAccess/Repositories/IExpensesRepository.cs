using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories;

public interface IExpensesRepository: IUnitOfWork
{
    /// <summary>
    /// Получить список расходов за период для пользователя
    /// </summary>
    /// <param name="userId"> Guid пользователя </param>
    /// <param name="startDate"> Дата начала периода </param>
    /// <param name="endDate">Дата окончания периода </param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<List<Expense>> GetExpensesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

    /// <summary>
    /// Получить список расходов за период для пользователя с учетом счета и валюты
    /// </summary>
    /// <param name="request">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список расходов</returns>
    Task<List<Expense>> GetExpensesByAccountAsync(ExpensesRequestDto request, CancellationToken cancellationToken);

    /// <summary>
    /// Получить сумму расходов до указанной даты включительно
    /// </summary>
    /// <param name="userId"> Guid пользователя </param>
    /// <param name="upToDate"> Дата, до которой получаем расходы (включительно)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns> Список расходов </returns>
    Task<decimal> GetSumOfExpensesUpToDateAsync(Guid userId, DateTime upToDate, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить новый расход
    /// </summary>
    /// <param name="expense">Расход</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task AddAsync(Expense expense, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получить расходы по категориям
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<CategoryAggregate>> GetExpenseTotalByCategoryAsync(
        Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удалить расход по ID
    /// </summary>
    /// <param name="expenceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteByIdAsync(Guid expenceId, CancellationToken cancellationToken);

    /// <summary>
    /// Изменить расход
    /// </summary>
    /// <param name="expenseId"></param>
    /// <param name="category"></param>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    /// <param name="expenseTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(
        Guid expenseId,
        string? category,
        string currency,
        decimal amount,
        DateTime expenseTime,
        CancellationToken cancellationToken);
}