using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data.Interfaces;

public interface ITransactionTagRepository
 {
     Task<TransactionTag> GetByTransactionIdAsync(Guid id, CancellationToken cancellationToken);
     Task AddAsync(TransactionTag transactionTag, CancellationToken cancellationToken);
     Task DeleteAsync(TransactionTag transactionTag, CancellationToken cancellationToken);
     
     Task UpdateAsync(TransactionTag transaction, CancellationToken cancellationToken);
 }