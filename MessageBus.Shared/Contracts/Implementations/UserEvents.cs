
using MessageBus.Shared.Contracts.Interfaces;

namespace MessageBus.Shared.Contracts.Implementations
{
    public record UserCreatedMessage(
        Guid UserId,
        string Name,
        Guid MessageId,
        DateTime Timestamp) : IUserCreated;

    public record UserDeletedMessage(
        Guid UserId,
        Guid MessageId,
        DateTime Timestamp) : IUserDeleted;
}
