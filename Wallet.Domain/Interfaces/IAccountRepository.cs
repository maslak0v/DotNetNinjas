using Wallet.Domain.Entities;

namespace Wallet.Domain.Interfaces;

public interface IAccountRepository 
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetAllByUserIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Account account, CancellationToken cancellationToken);
    Task Update(Account account, CancellationToken cancellationToken);
    Task SoftDelete(Account account, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}