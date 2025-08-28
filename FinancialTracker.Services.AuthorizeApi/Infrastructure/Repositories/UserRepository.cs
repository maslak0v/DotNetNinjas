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
        public async Task<bool> ExistEmailAsync(string email, CancellationToken cancellationToken)
            => (await userManager.FindByEmailAsync(email)) is not null;

        public async Task<bool> ExistUserNameAsync(string userName, CancellationToken cancellationToken)
            => (await userManager.FindByNameAsync(userName)) is not null;

        public async Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersQueryAsync(CancellationToken cancellationToken)
        {
            List<IUserResponseInfo> data = await userManager.Users
                .AsNoTracking()
                .ToResponseFormatExt()
                .ToListAsync(cancellationToken);
            return OperationResultCreator.Success(data, Enum_StatusCode.OK);
        }

        public async Task<User?> TryGetCurrentLoginUserAsync(string email, string password, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null || !await userManager.CheckPasswordAsync(user, password))
                return null;
            var roles = await userManager.GetRolesAsync(user);
            return user.ToDomainUser(roles);
        }
        public async Task<IList<string>> GetRolesForUserAsync(User domainUser, CancellationToken cancellationToken)
        {
            var user = domainUser.ToAuthUser();
            return await userManager.GetRolesAsync(user);
        }


        public async Task<OperationResult<User>> RegisterUserAsync(
            IUserRegisterRequest request, ICollection<string> roles, CancellationToken cancellationToken)
        {
            await using var transaction = await authDb.Database.BeginTransactionAsync();
            var result = await CreateUserAsync(request, cancellationToken);
            if (!result.IsSuccess)
            {
                await transaction.RollbackAsync(cancellationToken);
                return OperationResultCreator.Failure<User>(result.StatusCode, result.Message!);
            }
            var user = result.Result;
            var resultAddRoles = await AddRolesToUserAsync(user!, roles);
            if (!resultAddRoles.IsSuccess)
            {
                await transaction.RollbackAsync(cancellationToken);
                return OperationResultCreator.FromOtherResult<User>(resultAddRoles);
            }
            await transaction.CommitAsync(cancellationToken);
            string resultMessage = $"{result?.Message} {resultAddRoles.Message}";
            return OperationResultCreator.Success(
                user!.ToDomainUser(roles), result!.StatusCode, resultMessage);
        }

        public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var model = await authDb.Users.AsNoTrackingWithIdentityResolution()
                 .Include(x => x.Roles)
                 .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (model is null)
                return null;
            IList<string> roles = model.Roles.Select(x => x.Name!).ToList();
            return model?.ToDomainUser(roles);
        }

        public async Task<OperationResult> AddRolesToUserAsync(User userDomain, ICollection<string> roles, CancellationToken cancellationToken)
        {
            var user = userDomain.ToAuthUser();
            return await AddRolesToUserAsync(user, roles);
        }
        private async Task<OperationResult> AddRolesToUserAsync(AuthUser user, ICollection<string> roles)
        {
            var result = await userManager.AddToRolesAsync(user, roles);
            string nameRoles = string.Join(",", roles);
            return result.Succeeded
                ? OperationResultCreator.Success(Enum_StatusCode.OK, $"with roles {nameRoles}")
                : OperationResultCreator.Failure(
                    Enum_StatusCode.BAD_REQUEST,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        private async Task<OperationResult<AuthUser>> CreateUserAsync(IUserRegisterRequest userDto, CancellationToken cancellationToken)
        {
            var user = new AuthUser().FromRegisterRequest(userDto);
            user.CreateAt = DateTime.UtcNow;
            var result = await userManager.CreateAsync(user, userDto.Password);
            return result.Succeeded
                ? OperationResultCreator.Success(user, Enum_StatusCode.CREATED, "User created successfully")
                : OperationResultCreator.Failure<AuthUser>(
                    Enum_StatusCode.BAD_REQUEST,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<OperationResult> DeleteAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return OperationResultCreator.Failure(Enum_StatusCode.NOT_FOUND, $"User with {userId} not found");
            var result = await userManager.DeleteAsync(user);
            return result.Succeeded
                ? OperationResultCreator.Success(Enum_StatusCode.NO_CONTENT)
                : OperationResultCreator.Failure(
                    Enum_StatusCode.BAD_REQUEST,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
