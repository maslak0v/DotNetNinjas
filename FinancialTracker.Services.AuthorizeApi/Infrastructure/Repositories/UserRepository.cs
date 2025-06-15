using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Repositories
{

    public class UserRepository(
        UserManager<AuthUser> userManager,
        AuthDbContext authDb) : IUserRepository

    {
        public async Task<OperationResult> CreateUserAsync(IUserRegisterRequest userDto)
        {
            var user = new AuthUser().FromRegisterRequest(userDto);
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
            if (user is null || !await userManager.CheckPasswordAsync(user, password))
                return null;
            var roles = await userManager.GetRolesAsync(user);
            return user.ToDomainUser(roles);
        }
        public async Task<IList<string>> GetRolesForUserAsync(User domainUser)
        {
            var user = domainUser.ToAuthUser();
            return await userManager.GetRolesAsync(user);
        }

        public async Task<OperationResult> AddRolesToUserAsync(string userName, ICollection<string> roles)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user is null)
                return OperationResultCreator.Failure(Enum_StatusCode.NOT_FOUND, "user not found");
            var result = await userManager.AddToRolesAsync(user, roles);
            string nameRoles = string.Join(",", roles);
            return result.Succeeded
                ? OperationResultCreator.Success(Enum_StatusCode.OK, $"with roles {nameRoles}")
                : OperationResultCreator.Failure(
                    Enum_StatusCode.BAD_REQUEST,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<OperationResult> RegisterUserAsync(
            IUserRegisterRequest request, ICollection<string> roles)
        {
            await using var transaction = await authDb.Database.BeginTransactionAsync();
            var result = await CreateUserAsync(request);
            if(!result.IsSuccess)
            {
                await transaction.RollbackAsync();
                return result;
            }
            var resultAdd = await AddRolesToUserAsync(request.FullName, roles);
            if (!resultAdd.IsSuccess)
            {
                await transaction.RollbackAsync();
                return resultAdd;
            }
            await transaction.CommitAsync();
            string messageAddRoles = resultAdd.Message ?? string.Empty;
            var newResult = result with { Message = $"{result?.Message ?? string.Empty} {messageAddRoles}" };            
            return newResult!;
        }

        public async Task<User?> FindByIdAsync(string userId)
        {
            var model = await authDb.Users.AsNoTrackingWithIdentityResolution()
                 .Include(x => x.Roles)
                 .FirstOrDefaultAsync(x => x.Id == userId);
            if(model is null)
                return null;
            IList<string> roles = model.Roles.Select(x => x.Name!).ToList();
            return model?.ToDomainUser(roles);
        }
    }
}
