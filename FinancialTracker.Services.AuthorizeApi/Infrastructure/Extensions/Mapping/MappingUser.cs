using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using MessageBus.Shared.Contracts.Implementations;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Extensions.Mapping
{
    public static class MappingUser
    {
        public static UserCreatedMessage ToUserCreatedMessage(this User user) => new UserCreatedMessage(
                Guid.Parse(user.Id),
                user.UserName,
                Guid.CreateVersion7(),
                user.CreateAt);
    }
}
