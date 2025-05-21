using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public interface IUseCaseFabric
    {
        IUserRegisterUseCase CreateUserRegisterAsync();
        IGetAllUsersUseCase CreateGetAllUsersAsync();

        //other usecases..
    }
}
