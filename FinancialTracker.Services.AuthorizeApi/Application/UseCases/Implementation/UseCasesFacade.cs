using AutoMapper;
using FinancialTracker.Services.AuthorizeApi.Application.Contracts;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class UseCasesFacade(
        UserManager<AuthUser> userManager,
        ITokenService<AuthUser> tokenSerice,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<UseCasesFacade> logger) : IUseCasesFacade
    {
        /// <summary>
        /// Registration a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UserRegisterAsync(UserRegisterRequest request)
        {
            logger.LogInformation("Registration of a new user");
            await checkExistUserAsync(request);

            var newUser = mapper.Map<AuthUser>(request);
            newUser.CreateAt = DateTime.Now; //maybe utc?
            var result = await userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                ErrorHandler.HandleWarningError(logger, $"Failed to create user: {errors}");
            }
            logger.LogInformation("User created successfully");


            //------------------ Footer --------------------------
            //helpers methods
            async Task checkExistUserAsync(UserRegisterRequest request)
            {
                //сначала проверить проверку уникальности из коробки Identity
                //if (await ExistEmailAsync(request.Email))
                //{
                //    ErrorHandler.HandleWarningError(logger, "Email already exist");
                //}

                if (await ExistUsernameAsync(request.FullName))
                {
                    ErrorHandler.HandleWarningError(logger, "User's name already exist");
                }
            }

            //maybe to specifications
            async Task<bool> ExistUsernameAsync(string fullname)
                => await userManager.FindByNameAsync(fullname) is not null;

            async Task<bool> ExistEmailAsync(string email)
                => await userManager.FindByEmailAsync(email) is not null;
        }

        public Task<UserResponse> UserLoginAsync(UserLoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UserLogoutAsync(UserLogoutRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> UserUpdateAsync(Guid id, UserUpdateRequest reqest)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CurrentUserLoginResponse> GetCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> GetUserByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        //public Task<CurrentUserLoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<RevokeRefreshTokenResponse> RevokeRefreshTokenAsync(RefreshTokenRemoveRequest request)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
