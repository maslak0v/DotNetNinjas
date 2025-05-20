

using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    public  static class UseCaseFabricCreator
    {
        public static IUseCaseFabric Create(IUserRepository repo) 
            => new UseCaseFabric(repo);
    }
}
