using Wallet.Application.Dto.Transactions;

namespace Wallet.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionDtoResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TransactionDtoResponse>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(TransactionDto createTransactionDto, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
