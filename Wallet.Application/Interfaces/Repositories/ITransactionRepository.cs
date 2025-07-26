using Wallet.Domain.Entities;

namespace Wallet.Application.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken, 
        bool includeRelated = true, 
        bool noTracking = false);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(Guid accountId, DateTime startDate, DateTime endDate, int limit, CancellationToken cancellationToken);
}