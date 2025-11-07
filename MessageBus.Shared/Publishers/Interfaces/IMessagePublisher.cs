using MessageBus.Shared.Contracts.Interfaces;

namespace MessageBus.Shared.Publishers.Interfaces
{
    public interface IMessagePublisher 
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
            where TMessage : class, IMessage;
    }
}
