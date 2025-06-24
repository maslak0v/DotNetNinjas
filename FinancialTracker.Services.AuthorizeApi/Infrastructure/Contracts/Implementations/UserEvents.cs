using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Implementations
{
    public record UserCreatedEvent(string UserId, DateTime CreateAt, string Name) : IUserEvent;
    public record UserDeletedEvent(string UserId) : IUserEvent;
}
