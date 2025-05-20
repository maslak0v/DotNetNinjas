using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Repositories
{
    public class UserRepository(UserManager<AuthUser> userManager) : IUserRepository
    {
        public async Task<OperationResult> CreateUserAsync(IUserRegisterRequest userRegisterRequest)
        {
            var user = new AuthUser().FromRegisterRequst(userRegisterRequest);
            user.CreateAt = DateTime.UtcNow;
            var result = await userManager.CreateAsync(user, userRegisterRequest.Password);
            return result.Succeeded
                ? OperationResultCreator.Success(Enum_StatusCode.CREATED, "User created successfully")
                : OperationResultCreator.Failure(
                    Enum_StatusCode.BAD_REQUEST,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<bool> ExistEmailAsync(string email)
            => await userManager.FindByEmailAsync(email) is not null;

        public async Task<bool> ExistUsernameAsync(string userName)
            => await userManager.FindByNameAsync(userName) is not null;
    }
}
