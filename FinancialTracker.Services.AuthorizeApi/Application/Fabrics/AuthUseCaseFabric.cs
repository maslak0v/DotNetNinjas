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
            IAuthTokenService service,
            IRefreshRequest request)
            => new RefreshTokenUseCase(repository, service, request);

        public ILogoutUseCase CreateLogout(string userId, IAuthTokenService service)
         => new LogoutUseCase(userId, service);

        public IRevokeAllUseCase CreateRevokeAll(IAuthTokenService service)
         => new RevokeAllUseCase(service);


        //other use-cases ..
    }
}
