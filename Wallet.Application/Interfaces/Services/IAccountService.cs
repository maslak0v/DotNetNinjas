using Wallet.Application.Dto.Accounts;
using Wallet.Application.Helpers;

namespace Wallet.Application.Interfaces.Services;

public interface IAccountService
{
    public Task<IEnumerable<AccountDto>> GetAllByUserIdAsync(Guid id, CancellationToken cancellationToken);
    public Task AddAsync(AccountDto account, CancellationToken cancellationToken);
    public Task UpdateAsync(AccountDto account, CancellationToken cancellationToken);
    public Task<OperationResult> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    public Task<AccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}