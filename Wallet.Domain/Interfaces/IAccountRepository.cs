using Wallet.Domain.Entities;

namespace Wallet.Domain.Interfaces;

public interface IAccountRepository 
{
    Task<Account?> GetByIdAsync(Guid id);
    Task<decimal> GetTotalBalanceByUserIdAsync(Guid userId);
    Task AddAsync(Account account);
    void Update(Account account);
    void Delete(Account account);
    Task<bool> ExistsAsync(Guid id);
}