using MassTransit;

namespace MessageBus.Shared.Contracts.Interfaces;

[ExcludeFromTopology]
public interface IExpenseDeletedMessage
{
    Guid ExpenseId { get; }
    Guid UserId { get; }
    Guid MessageId { get; }
    DateTime Timestamp { get; }
}
