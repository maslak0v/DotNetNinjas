using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Implementations;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping
{
    public static class MappingAuthUser
    {
        public static AuthUser FromRegisterRequest(this AuthUser user, IUserRegisterRequest userDto)
        {
            user.Email = userDto.Email;
            user.UserName = userDto.FullName;
            return user;
        }
        public static IQueryable<IUserResponseInfo> ToResponseFormatExt(this IQueryable<AuthUser> query)
             => query.Select(x => new UserResponseInfo(x.Id, x.UserName!, x.Email!));

        public static User ToDomainUser(this AuthUser user, ICollection<string> roles) => new()
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            CreateAt = user.CreateAt,
            UpdateAt = user.UpdateAt,
            Roles = roles.ToList() ?? []
        };

        public static AuthUser ToAuthUser(this User user) => new()
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            CreateAt = user.CreateAt,
            UpdateAt = user.UpdateAt
        };
    }
}
