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
    public record IncomeUpdateMessage(
        Guid IncomeId,
        Guid UserId,
        Guid AccountId,
        string? Category,
        string Currency,
        decimal Amount,
        DateTime IncomeTime,
        Guid MessageId,
        DateTime UpdateTime) : IIncomeUpdateMessage;

    [ExcludeFromTopology]
    public record IncomeDeletedMessage(
        Guid IncomeId,
        Guid UserId,
        Guid MessageId,
        DateTime Timestamp) : IIncomeDeletedMessage;

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
    public record ExpenseUpdateMessage(
        Guid ExpenseId,
        Guid UserId,
        Guid AccountId,
        string? Category,
        string Currency,
        decimal Amount,
        DateTime ExpenseTime,
        Guid MessageId,
        DateTime UpdateTime) : IExpenseUpdateMessage;

    [ExcludeFromTopology]
    public record ExpenseDeletedMessage(
        Guid ExpenseId,
        Guid UserId,
        Guid MessageId,
        DateTime Timestamp) : IExpenseDeletedMessage;
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
