using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public interface IAuthUseCaseFabric
    {
        IUserRegisterUseCase CreateUserRegisterAsync();
        IGetAllUsersUseCase CreateGetAllUsersAsync();

        //other usecases..
    }
}
