using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public class AuthUseCaseFabric(
        IUserRepository repository,
        ITokenService tokenService): IAuthUseCaseFabric
    {
        public IGetAllUsersUseCase CreateGetAllUsers()
            => new GetAllUsersUseCase(repository);

        public IUserRegisterUseCase CreateUserRegister(IUserRegisterRequest request)
            => new UserRegisterUseCase(repository, request);

        public ILoginUseCase CreateLogin(IUserLoginRequest request)
            => new LoginUseCase(repository, tokenService, request);

        //other use-cases ..
    }
}
