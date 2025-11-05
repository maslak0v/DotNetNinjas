using AutoMapper;
using Wallet.Application.Dto.Accounts;
using Wallet.Application.Helpers;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Application.Interfaces.Services;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Services;

public class AccountService : IAccountService
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

    public async Task<OperationResult> UpdateAsync(AccountDto account, CancellationToken cancellationToken)
    {
        var updatedAccount = await _accountRepository.GetByIdAsync(account.AccountId, cancellationToken);
        if (updatedAccount is null)
            return OperationResult.Failure(Enum_StatusCode.NotFound, $"id:{account.AccountId} not found");

        updatedAccount.Name = account.Name;
        updatedAccount.CurrentBalance = account.CurrentBalance;
        updatedAccount.UpdatedAt = account.UpdatedAt;

        await _accountRepository.UpdateAsync(updatedAccount, cancellationToken);
        return OperationResult.Success(Enum_StatusCode.NoContent);
    }

    public async Task<OperationResult> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deletedAccount = await _accountRepository.GetByIdAsync(id, cancellationToken);
        if (deletedAccount is null)
            return OperationResult.Failure(Enum_StatusCode.NotFound, $"id:{id} not found");
        deletedAccount.IsDeleted = true;
        await _accountRepository.SoftDelete(deletedAccount, cancellationToken);
        return OperationResult.Success(Enum_StatusCode.NoContent);
    }
    public async Task<AccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<AccountDto>(account);
    }
}