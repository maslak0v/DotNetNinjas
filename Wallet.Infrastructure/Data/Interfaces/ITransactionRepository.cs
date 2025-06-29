using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(Guid accountId, DateTime startDate, DateTime endDate, int limit, CancellationToken cancellationToken);
}