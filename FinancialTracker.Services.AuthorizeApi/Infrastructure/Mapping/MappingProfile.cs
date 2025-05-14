using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping
{
    public static class MappingProfile
    {
        public static AuthUser FromRegisterRequst(this AuthUser user, IUserRegisterRequest userDto)
        {
            user.Email = userDto.Email;
            user.UserName = userDto.FullName;
            return user;
        }
    }
}
