using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace MessageBus.Shared.Contracts.Implementations;

public class TransactionEvents
{
    [ExcludeFromTopology]
    public record TransactionDeletedMessage(
        Guid MessageId,
        DateTime Timestamp,
        Guid TransactionId,
        String TransactionType
    ) : IDeleteTransactionMessage;

    [ExcludeFromTopology]
    public record IncomeCreatedMessage(
        Guid MessageId,
        DateTime Timestamp,
        Guid IncomeId,
        Guid UserId,
        Guid AccountId,
        string CategoryName,
        DateTime IncomeTime,
        decimal Amount) : IIncomeCreatedMessage;

    [ExcludeFromTopology]
    public record ExpenseCreatedMessage(
        Guid MessageId,
        DateTime Timestamp,
        Guid ExpenseId,
        Guid UserId,
        Guid AccountId,
        string CategoryName,
        DateTime ExpenseTime,
        decimal Amount) : IExpenseCreatedMessage;

    [ExcludeFromTopology]
    public record UpdateTransactionMessage(
        Guid MessageId,
        DateTime Timestamp,
        Guid TransactionId,
        Guid UserId,
        Guid AccountId,
        string CategoryName,
        DateTime TransactionDate,
        decimal Amount,
        string OperationType,
        string OldOperationType) : IUpdateTransactionMessage;
}
