using Wallet.Domain.Entities;

namespace Wallet.Domain.Interfaces;

public interface ITransactionTagRepository
{
    Task AddAsync(TransactionTag transactionTag);
    Task<bool> ExistsAsync(Guid transactionId, Guid tagId);
}