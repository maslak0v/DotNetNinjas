using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Services;
using FinancialTracker.Services.Analytics.Services.Implementation;

namespace FinancialTracker.Services.Analytics;

public static class Registrator
{
    public static IServiceCollection InstallServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IExpensesService, ExpensesService>();
        return serviceCollection;
    }

    public static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IExpensesRepository, ExpensesRepository>();

        return serviceCollection;
    }
}