using MassTransit;

namespace MessageBus.Shared.Contracts.Interfaces
{
    [ExcludeFromTopology]
    public interface IMessage
    {
        Guid MessageId { get; }
        DateTime Timestamp {  get; }
    }
}
