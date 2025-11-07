

namespace MessageBus.Shared.Contracts.Interfaces
{
    public interface IUserCreated: IMessage
    {
        Guid UserId { get; }
        string Name { get; }
    }
    public interface IUserDeleted: IMessage
    {
        Guid UserId { get; }
    }
}
