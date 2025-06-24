using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Implementations
{
    public record UserCreatedEvent(string Id, DateTime CreateAt, string Name) : IUserEvent;
    public record UserUpdatedEvent(string Id, DateTime UpdateAt) : IUserEvent;
    public record UserDeletedEvent(string Id) : IUserEvent;

    //other events
}
