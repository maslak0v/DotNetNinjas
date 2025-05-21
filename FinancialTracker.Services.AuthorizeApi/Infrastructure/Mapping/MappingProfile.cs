using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts;
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
        public static IQueryable<IUserResponseInfo> ToResponseFormatExt(this IQueryable<AuthUser> query)
             => query.Select(x => new UserResponseInfo(x.Id, x.UserName!, x.Email!));
    }
}
