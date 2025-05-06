using FinancialTracker.Services.Analytics.Models;

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
}