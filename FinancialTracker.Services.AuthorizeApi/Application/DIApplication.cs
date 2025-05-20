using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Application
{
    public static class DIApplication
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {

            services.AddScoped<IUseCaseFabric, UseCaseFabric>();
            services.AddScoped<IUseCasesFacade, UseCasesFacade>();
            
            return services;
        }
    }
}
