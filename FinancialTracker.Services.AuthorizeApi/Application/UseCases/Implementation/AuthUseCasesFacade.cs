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
        public async Task<OperationResult<User>> UserRegisterAsync(IUserRepository userRepository, IUserRegisterRequest request, CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<IUserRegisterUseCase, OperationResult<User>>(
                () => useCaseFabric.CreateUserRegister(userRepository, request), cancellationToken);

        public async Task<OperationResult<ITokenResponse>> UserLoginAsync(
            IUserRepository userRepository,
            IAuthTokenService tokenService,
            IUserLoginRequest request,
            CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<ILoginUseCase, OperationResult<ITokenResponse>>(
                () => useCaseFabric.CreateLogin(userRepository, tokenService, request), cancellationToken);

        public async Task<OperationResult> DeleteAsync(
            string userId,
            IUserRepository userRepository,
            CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<IDeleteUseCase, OperationResult>(
                () => useCaseFabric.CreateDeleteUseCase(userId, userRepository), cancellationToken);

        public Task<OperationResult<ICurrentUserLoginResponse>> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> UserLogoutAsync(string userId, IAuthTokenService tokenService, CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<ILogoutUseCase, OperationResult>(
                () => useCaseFabric.CreateLogout(userId, tokenService), cancellationToken);


        public async Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersAsync(
            IUserRepository userRepository,
            CancellationToken cancellationToken) 
            => await ExecuteUseCaseAsync<IGetAllUsersUseCase,
                OperationResult<List<IUserResponseInfo>>> (() => useCaseFabric.CreateGetAllUsers(userRepository), cancellationToken);


        public async Task<OperationResult<ITokenResponse>> RefreshAsync(
            IUserRepository userRepository,
            IAuthTokenService tokenService,
            IRefreshRequest request,
            CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<IRefreshUseCase, OperationResult<ITokenResponse>>(
            () => useCaseFabric.CreateRefresh(userRepository, tokenService, request), cancellationToken);

        public async Task<OperationResult> RevokeAllRefreshTokensAsync(
            IAuthTokenService tokenService,
            CancellationToken cancellationToken)
            => await ExecuteUseCaseAsync<IRevokeAllUseCase, OperationResult>(
                () => useCaseFabric.CreateRevokeAll(tokenService), cancellationToken);

        private async Task<TResult> ExecuteUseCaseAsync<TUseCase, TResult>(
            Func<TUseCase> createUseCase,
            CancellationToken cancellationToken) where TUseCase : ICommandAsync<TResult>
        {
            var useCase = createUseCase();
            await useCase.ExecuteAsync(cancellationToken);
            return useCase.Result;
        }
    }
}
