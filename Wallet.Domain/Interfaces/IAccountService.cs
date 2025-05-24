using Wallet.Domain.Entities;

namespace Wallet.Domain.Interfaces;

public interface IAccountService
{
    public Task<IEnumerable<Account>> GetAllByUserIdAsync(Guid id, CancellationToken cancellationToken);
    public Task AddAsync(Account account, CancellationToken cancellationToken);
    public Task UpdateAsync(Account account, CancellationToken cancellationToken);
    public Task DeleteAsync(Account account, CancellationToken cancellationToken);
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    public Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}