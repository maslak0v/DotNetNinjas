using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public class AuthUseCaseFabric(
        IUserRepository repository) : IAuthUseCaseFabric
    {
        public IGetAllUsersUseCase CreateGetAllUsers()
            => new GetAllUsersUseCase(repository);

        public IUserRegisterUseCase CreateUserRegister(IUserRegisterRequest request)
            => new UserRegisterUseCase(repository, request);

        public ILoginUseCase CreateLogin(IAuthTokenService tokenService, IUserLoginRequest request)
            => new LoginUseCase(repository, tokenService, request);

        public IRefreshUseCase CreateRefresh(IAuthTokenService service, IRefreshRequest request)
            => new RefreshTokenUseCase(repository, service, request);

        public ILogoutUseCase CreateLogout(string userId, IAuthTokenService service)
         => new LogoutUseCase(userId, service);


        //other use-cases ..
    }
}
