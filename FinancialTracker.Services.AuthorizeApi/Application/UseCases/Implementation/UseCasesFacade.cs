using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class UseCasesFacade(IUseCaseFabric useCaseFabric) : IUseCasesFacade
    {

        /// <summary>
        /// Registration a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OperationResult> UserRegisterAsync(IUserRegisterRequest request)
        {
            var useCase = useCaseFabric.CreateUserRegisterAsync();
            useCase.Request = request;
            await useCase.Execute();
            return useCase.Result;
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

        public async Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersAsync()
        {
            var useCase = useCaseFabric.CreateGetAllUsersAsync();
            await useCase.Execute();
            var result = useCase.Result;
            return result;
        }
    }
}
