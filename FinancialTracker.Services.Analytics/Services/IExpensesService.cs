using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services;

public interface IExpensesService
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
    /// Получить сумму расходов до указанной даты (включительно) для пользователя
    /// </summary>
    /// <param name="userId"> Guid пользователя </param>
    /// <param name="upToDate"> Дата, до которой получаем расходы (включительно)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns> Список расходов </returns>
    Task<decimal> GetSumOfExpensesUpToDateAsync(Guid userId, DateTime upToDate, CancellationToken cancellationToken);

    /// <summary>
    /// Получить список расходов за период для пользователя с учетом счета и валюты
    /// </summary>
    /// <param name="request">Параметры запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список расходов</returns>
    Task<List<Expense>> GetExpensesByAccountAsync(ExpensesRequestDto request, CancellationToken cancellationToken);

    /// <summary>
    /// Добавить новый расход
    /// </summary>
    /// <param name="expense">Расход</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task AddAsync(ExpenseDto expense, CancellationToken cancellationToken);
}