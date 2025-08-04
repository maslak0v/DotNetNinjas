
using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    public static class AuthUseCaseFabricCreator
    {
        public static IAuthUseCaseFabric Create()
            => new AuthUseCaseFabric();
    }
}
