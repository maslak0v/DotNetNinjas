using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public class AuthUseCaseFabric : IAuthUseCaseFabric
    {
        public IGetAllUsersUseCase CreateGetAllUsers(IUserRepository repository)
            => new GetAllUsersUseCase(repository);

        public IUserRegisterUseCase CreateUserRegister(
            IUserRepository repository,
            IUserRegisterRequest request)
            => new UserRegisterUseCase(repository, request);

        public ILoginUseCase CreateLogin(
            IUserRepository repository,
            IAuthTokenService tokenService,
            IUserLoginRequest request)
            => new LoginUseCase(repository, tokenService, request);

        public IRefreshUseCase CreateRefresh(
            IUserRepository repository,
            IAuthTokenService tokenService,
            IRefreshRequest request)
            => new RefreshTokenUseCase(repository, tokenService, request);

        public ILogoutUseCase CreateLogout(string userId, IAuthTokenService tokenService)
         => new LogoutUseCase(userId, tokenService);

        public IRevokeAllUseCase CreateRevokeAll(IAuthTokenService tokenService)
         => new RevokeAllUseCase(tokenService);

        public IDeleteUseCase CreateDeleteUseCase(string userId, IUserRepository repository)
            => new DeleteUseCase(userId, repository);

        //other use-cases ..
    }
}
