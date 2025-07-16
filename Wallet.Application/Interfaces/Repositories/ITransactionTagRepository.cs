using Wallet.Domain.Entities;

namespace Wallet.Application.Interfaces.Repositories;

public interface ITransactionTagRepository
 {
     Task<TransactionTag> GetByTransactionIdAsync(Guid id, CancellationToken cancellationToken);
     Task AddAsync(TransactionTag transactionTag, CancellationToken cancellationToken);
     Task DeleteAsync(TransactionTag transactionTag, CancellationToken cancellationToken);
     
     Task UpdateAsync(TransactionTag transaction, CancellationToken cancellationToken);
 }