using MassTransit;

namespace MessageBus.Shared.Contracts.Interfaces;

[ExcludeFromTopology]
public interface IIncomeDeletedMessage
{
    Guid IncomeId { get; }
    Guid UserId { get; }
    Guid MessageId { get; }
    DateTime Timestamp { get; }
}
