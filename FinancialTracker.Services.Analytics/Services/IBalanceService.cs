namespace FinancialTracker.Services.Analytics.Services;

public interface IBalanceService
{
    Task<decimal> GetBalanceAsync(Guid userId, DateTime forDate, CancellationToken cancellationToken);
}