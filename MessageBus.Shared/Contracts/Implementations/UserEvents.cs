
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace MessageBus.Shared.Contracts.Implementations
{
    [ExcludeFromTopology]
    public record UserCreatedMessage(
        Guid UserId,
        string Name,
        Guid MessageId,
        DateTime Timestamp) : IUserCreated;

    [ExcludeFromTopology]
    public record UserDeletedMessage(
        Guid UserId,
        Guid MessageId,
        DateTime Timestamp) : IUserDeleted;
}
