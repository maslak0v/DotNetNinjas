using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class AuthUseCasesFacade(IAuthUseCaseFabric useCaseFabric) : IAuthUseCasesFacade
    {

        /// <summary>
        /// Registration a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OperationResult> UserRegisterAsync(IUserRegisterRequest request)
            => await ExecuteUseCaseAsync<IUserRegisterUseCase, OperationResult>(
                () => useCaseFabric.CreateUserRegister(request));

        public async Task<OperationResult<IAuthResponse>> UserLoginAsync(ITokenService service, IUserLoginRequest request)
            => await ExecuteUseCaseAsync<ILoginUseCase, OperationResult<IAuthResponse>>(
                () => useCaseFabric.CreateLogin(service, request));

        public Task<OperationResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<ICurrentUserLoginResponse>> GetCurrentUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IAuthResponse>> GetUserByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

       

        public Task<OperationResult> UserLogoutAsync(IUserLogoutRequest request)
        {
            throw new NotImplementedException();
        }


        public Task<OperationResult<IAuthResponse>> UserUpdateAsync(IUserUpdateRequest reqest)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersAsync() 
            => await ExecuteUseCaseAsync<IGetAllUsersUseCase,
                OperationResult<List<IUserResponseInfo>>> (useCaseFabric.CreateGetAllUsers);

        private async Task<TResult> ExecuteUseCaseAsync<TUsecase, TResult>(
            Func<TUsecase> createUsecase) where TUsecase : ICommandAsync<TResult>
        {
            var usecase = createUsecase();
            await usecase.ExecuteAsync();
            return usecase.Result;
        }
    }
}
