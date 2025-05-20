using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Application.Fabrics
{
    public class UseCaseFabric(IUserRepository repository): IUseCaseFabric
    {
        public IUserRegisterUseCase CreateUserRegister_UseCase()
            => new UserRegisterUseCase(repository);

        //other use-cases ..
    }
}
