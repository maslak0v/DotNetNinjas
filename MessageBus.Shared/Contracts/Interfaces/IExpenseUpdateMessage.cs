namespace MessageBus.Shared.Contracts.Interfaces;

public interface IExpenseUpdateMessage
{
    Guid ExpenseId { get; }
    Guid UserId { get; }
    Guid AccountId { get; }
    string? Category { get; }
    string Currency { get; }
    decimal Amount { get; }
    DateTime ExpenseTime { get; }
    Guid MessageId { get; }
    DateTime UpdateTime { get; }
}
