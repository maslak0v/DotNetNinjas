using Wallet.Application.Dto.Transactions;
using Wallet.Application.Helpers;
using Wallet.Domain.Enums;

namespace Wallet.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<OperationResult<TransactionDtoResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<OperationResult<IEnumerable<TransactionDtoResponse>>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task<OperationResult<TransactionDtoResponse>> CreateAsync(TransactionDto createTransactionDto, CancellationToken cancellationToken);
    Task<OperationResult<TransactionDtoResponse>> UpdateAsync(Guid id, TransactionDto transactionDto, CancellationToken cancellationToken);
    Task<OperationResult<DeleteTransactionDto>> DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    decimal CalculateUpdatedBalance(
        decimal currentBalance,
        decimal amount,
        OperationType operationType,
        bool isAdding);
}
