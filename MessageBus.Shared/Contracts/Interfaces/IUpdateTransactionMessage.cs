namespace MessageBus.Shared.Contracts.Interfaces;

public interface IUpdateTransactionMessage
{
    Guid MessageId { get; }
    DateTime Timestamp { get; }
    Guid TransactionId { get; }
    Guid UserId { get; }
    Guid AccountId { get; }
    string CategoryName { get; }
    DateTime TransactionDate { get; }
    decimal Amount { get; }
    string OperationType { get; }
    string OldOperationType { get; }
}
