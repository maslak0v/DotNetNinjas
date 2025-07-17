using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
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
        public async Task<OperationResult<User>> UserRegisterAsync(IUserRegisterRequest request, CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<IUserRegisterUseCase, OperationResult<User>>(
                () => useCaseFabric.CreateUserRegister(request), cancellationToken);

        public async Task<OperationResult<ITokenResponse>> UserLoginAsync(
            IAuthTokenService service, IUserLoginRequest request, CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<ILoginUseCase, OperationResult<ITokenResponse>>(
                () => useCaseFabric.CreateLogin(service, request), cancellationToken);

        public Task<OperationResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<ICurrentUserLoginResponse>> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> UserLogoutAsync(string userId, IAuthTokenService service, CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<ILogoutUseCase, OperationResult>(
                () => useCaseFabric.CreateLogout(userId, service), cancellationToken);


        public async Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersAsync(CancellationToken cancellationToken) 
            => await ExecuteUseCaseAsync<IGetAllUsersUseCase,
                OperationResult<List<IUserResponseInfo>>> (useCaseFabric.CreateGetAllUsers, cancellationToken);


        public async Task<OperationResult<ITokenResponse>> RefreshAsync(
            IAuthTokenService service, IRefreshRequest request, CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<IRefreshUseCase, OperationResult<ITokenResponse>>(
            () => useCaseFabric.CreateRefresh(service, request), cancellationToken);


        private async Task<TResult> ExecuteUseCaseAsync<TUsecase, TResult>(
            Func<TUsecase> createUsecase,
            CancellationToken cancellationToken) where TUsecase : ICommandAsync<TResult>
        {
            var usecase = createUsecase();
            await usecase.ExecuteAsync(cancellationToken);
            return usecase.Result;
        }
    }
}
