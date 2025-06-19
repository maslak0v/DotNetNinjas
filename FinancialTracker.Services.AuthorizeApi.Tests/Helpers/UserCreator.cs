using FinancialTracker.Services.AuthorizeApi.Domain.Entities;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    internal static class UserCreator
    {
        public static User CreateWithUserRole() => new User()
        {
            Id = Guid.CreateVersion7().ToString(),
            CreateAt = DateTime.UtcNow,
            Email = "user@mail.ru",
            UserName = "User",
            Roles = ["USER"]
        };
    }
}
