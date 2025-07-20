using Wallet.Domain.Entities;

namespace Wallet.Application.Interfaces.Repositories;

public interface ITransactionTagRepository
 {
     Task<TransactionTag?> GetByTransactionIdAsync(
         Guid id, 
         CancellationToken cancellationToken, 
         bool noTracking = true);
     Task AddAsync(TransactionTag transactionTag, CancellationToken cancellationToken);
     Task DeleteAsync(Guid id, CancellationToken cancellationToken);
     
     Task UpdateAsync(TransactionTag transaction, CancellationToken cancellationToken);
 }