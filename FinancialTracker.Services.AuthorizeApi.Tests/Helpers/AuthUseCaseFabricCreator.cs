
using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    public  static class AuthUseCaseFabricCreator
    {
        public static IAuthUseCaseFabric Create(IUserRepository repo) 
            => new AuthUseCaseFabric(repo);
    }
}
