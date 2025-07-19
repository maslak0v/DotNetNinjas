using Wallet.Application.Dto.Transactions;
using Wallet.Application.Helpers;

namespace Wallet.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<TransactionDtoResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TransactionDtoResponse>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task<OperationResult<Guid>> CreateAsync(TransactionDto createTransactionDto, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken);
    Task<OperationResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
