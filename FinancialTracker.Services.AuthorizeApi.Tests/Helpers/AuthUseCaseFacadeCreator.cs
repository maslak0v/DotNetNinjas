
using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    public static class AuthUseCaseFacadeCreator
    {
        public static IAuthUseCasesFacade Create(IAuthUseCaseFabric fabric)
            => new AuthUseCasesFacade(fabric);
    }
}
