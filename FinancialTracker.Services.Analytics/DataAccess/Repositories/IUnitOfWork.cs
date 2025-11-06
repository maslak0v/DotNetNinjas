using Microsoft.EntityFrameworkCore.Storage;

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> CreateTransactionAsync(CancellationToken ct);
    }
}
