using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;

namespace Wallet.Application.Services;

public class AccountService :  IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<IEnumerable<Account>> GetAllByUserIdAsync(Guid id, CancellationToken cancellationToken) => await _accountRepository.GetAllByUserIdAsync(id, cancellationToken);
    
    public async Task AddAsync(Account account, CancellationToken cancellationToken) 
        => await _accountRepository.AddAsync(account, cancellationToken);


    public async Task UpdateAsync(Account account, CancellationToken cancellationToken) 
        => await _accountRepository.Update(account, cancellationToken);

    public async Task SoftDeleteAsync(Account account, CancellationToken cancellationToken) => await _accountRepository.SoftDelete(account, cancellationToken);
    
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await _accountRepository.GetByIdAsync(id, cancellationToken);
}