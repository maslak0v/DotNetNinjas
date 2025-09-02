using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public interface IAuthUseCaseFabric
    {
        IUserRegisterUseCase CreateUserRegister(IUserRepository repository, IUserRegisterRequest request);
        IGetAllUsersUseCase CreateGetAllUsers(IUserRepository repository);
        ILoginUseCase CreateLogin(IUserRepository repository, IAuthTokenService tokenService, IUserLoginRequest request);
        IRefreshUseCase CreateRefresh(IUserRepository repository, IAuthTokenService tokenService, IRefreshRequest request);
        ILogoutUseCase CreateLogout(string userId, IAuthTokenService tokenService);
        IRevokeAllUseCase CreateRevokeAll(IAuthTokenService tokenService);
        IDeleteUseCase CreateDeleteUseCase(string userId, IUserRepository repository);
        //other usecases..
    }
}
