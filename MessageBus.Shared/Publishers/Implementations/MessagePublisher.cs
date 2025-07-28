using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;
using MessageBus.Shared.Publishers.Interfaces;

namespace MessageBus.Shared.Publishers.Imlementations
{
    public class MessagePublisher(
        IPublishEndpoint publishEndpoint)
        : IMessagePublisher
    {
        public async Task PublishAsync<TMessage>(TMessage userEvent, CancellationToken cancellationToken) 
            where TMessage : class, IMessage => await publishEndpoint.Publish(userEvent, cancellationToken);
    }
}
