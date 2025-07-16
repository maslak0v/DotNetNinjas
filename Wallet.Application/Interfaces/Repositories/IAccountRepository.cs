using Wallet.Domain.Entities;

namespace Wallet.Application.Interfaces.Repositories;

public interface IAccountRepository 
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetAllByUserIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Account account, CancellationToken cancellationToken);
    Task UpdateAsync(Account account, CancellationToken cancellationToken);
    Task SoftDelete(Account account, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}