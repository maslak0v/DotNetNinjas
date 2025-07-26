using Wallet.Domain.Enums;

namespace Wallet.Application.Dto.Transactions;

public class DeleteTransactionDto
{
    public Guid TransactionId { get; set; }
    public OperationType OperationType { get; set; }
}