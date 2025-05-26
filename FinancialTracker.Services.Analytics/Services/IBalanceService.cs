namespace FinancialTracker.Services.Analytics.Services;

public interface IBalanceService
{
    decimal GetBalance(Guid userId, DateTime forDate);
}