using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Repositories
{
    
    public class UserRepository(
        UserManager<AuthUser> userManager,
        AuthDbContext authDb): IUserRepository
        
    {
        public async Task<OperationResult> CreateUserAsync(IUserRegisterRequest userDto)
        {
            var user = new AuthUser().FromRegisterRequst(userDto);
            user.CreateAt = DateTime.UtcNow;
            var result = await userManager.CreateAsync(user, userDto.Password);
            return result.Succeeded
                ? OperationResultCreator.Success(Enum_StatusCode.CREATED, "User created successfully")
                : OperationResultCreator.Failure(
                    Enum_StatusCode.BAD_REQUEST,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<bool> ExistEmailAsync(string email)
            => (await userManager.FindByEmailAsync(email)) is not null;

        public async Task<bool> ExistUsernameAsync(string userName)
            => (await userManager.FindByNameAsync(userName)) is not null;

        public async Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersQueryAsync()
        {
            List<IUserResponseInfo> data = await userManager.Users
                .AsNoTracking()
                .ToResponseFormatExt()
                .ToListAsync();
            return OperationResultCreator.Success(data, Enum_StatusCode.OK);
        }

        public async Task<User?> TryGetCurrentLoginUserAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null || await userManager.CheckPasswordAsync(user, password))
                return null;
            return user.ToDomainUser();
        }
        public async Task<IList<string>> GetRolesForUserAsync(User domainUser)
        {
            var user = domainUser.ToAuthUser();
            return await userManager.GetRolesAsync(user);
        }
    }
}
