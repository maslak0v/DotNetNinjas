using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public interface IAuthUseCaseFabric
    {
        IUserRegisterUseCase CreateUserRegister(IUserRegisterRequest request);
        IGetAllUsersUseCase CreateGetAllUsers();
        ILoginUseCase CreateLogin(IAuthTokenService tokenService, IUserLoginRequest request);
        IRefreshUseCase CreateRefresh(IAuthTokenService service, IRefreshRequest request);
        ILogoutUseCase CreateLogout(string userId, IAuthTokenService service);
        //other usecases..
    }
}
