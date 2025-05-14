using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class UseCasesFacade(
        IUserRegisterUseCase userRegisterUseCase) : IUseCasesFacade
    {

        /// <summary>
        /// Registration a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OperationResult> UserRegisterAsync(IUserRegisterRequest request)
        {
            userRegisterUseCase.Request = request;
            await userRegisterUseCase.Execute();
            return userRegisterUseCase.Result;
        }

        public Task<OperationResult<IUserResponse>> UserLoginAsync(IUserLoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<ICurrentUserLoginResponse>> GetCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IUserResponse>> GetUserByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

       

        public Task<OperationResult> UserLogoutAsync(IUserLogoutRequest request)
        {
            throw new NotImplementedException();
        }


        public Task<OperationResult<IUserResponse>> UserUpdateAsync(IUserUpdateRequest reqest)
        {
            throw new NotImplementedException();
        }
    }
}
