using AutoMapper;
using Wallet.Application.Dto.Accounts;
using Wallet.Application.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Data.Interfaces;

namespace Wallet.Application.Services;

public class AccountService :  IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AccountDto>> GetAllByUserIdAsync(Guid id, CancellationToken cancellationToken)
    { 
       var accounts = await _accountRepository.GetAllByUserIdAsync(id, cancellationToken);
       var result = _mapper.Map<IEnumerable<AccountDto>>(accounts);
       return result;
    }

    public async Task AddAsync(AccountDto account, CancellationToken cancellationToken)
    {
        var newAccount = _mapper.Map<Account>(account);
        newAccount.CreatedAt = DateTime.UtcNow;
        await _accountRepository.AddAsync(newAccount, cancellationToken);
    }

    public async Task UpdateAsync(AccountDto account, CancellationToken cancellationToken)
    { 
        var updatedAccount = _mapper.Map<Account>(account);
        await _accountRepository.UpdateAsync(updatedAccount, cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deletedAccount = await _accountRepository.GetByIdAsync(id, cancellationToken);
        if (deletedAccount != null)
        {
            deletedAccount.IsDeleted = true;
            await _accountRepository.SoftDelete(deletedAccount, cancellationToken);
        }
    }
    
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) 
        => await _accountRepository.ExistsAsync(id, cancellationToken);

    public async Task<AccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<AccountDto>(account);
    }
}