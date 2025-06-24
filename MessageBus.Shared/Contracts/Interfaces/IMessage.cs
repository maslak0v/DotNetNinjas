namespace MessageBus.Shared.Contracts.Interfaces
{
    public interface IMessage
    {
        Guid MessageId { get; }
        DateTime Timestamp {  get; }
    }
}
