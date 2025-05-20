using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public interface IUseCaseFabric
    {
        IUserRegisterUseCase CreateUserRegister_UseCase();
    }
}
