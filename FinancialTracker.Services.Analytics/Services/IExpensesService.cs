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
    /// <returns></returns>
    IEnumerable<Expense> GetExpenses(Guid userId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получить список расходов до указанной даты (включительно) для пользователя
    /// </summary>
    /// <param name="userId"> Guid пользователя </param>
    /// <param name="beforeDate"> Дата, до которой получаем расходы </param>
    /// <returns> Список расходов </returns>
    IQueryable<Expense> GetExpensesBeforeDate(Guid userId, DateTime beforeDate);

    /// <summary>
    /// Получить список расходов за период для пользователя с учетом счета и валюты
    /// </summary>
    /// <param name="request">Параметры запроса</param>
    /// <returns>Список расходов</returns>
    IEnumerable<Expense> GetExpensesByAccount(ExpensesRequestDto request);
}