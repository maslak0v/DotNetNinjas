using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Mapping;
using FinancialTracker.Services.Analytics.Services;
using FinancialTracker.Services.Analytics.Services.Implementation;

namespace FinancialTracker.Services.Analytics;

public static class Registrator
{
    public static IServiceCollection InstallServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IExpensesService, ExpensesService>()
            .AddScoped<IBalanceService, BalanceService>()
            .AddScoped<IRevenuesService, RevenueService>();
        return serviceCollection;
    }

    public static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IExpensesRepository, ExpensesRepository>()
            .AddScoped<IRevenuesRepository, RevenuesRepository>();

        return serviceCollection;
    }
    
    public static IServiceCollection InstallAutoMapper(this IServiceCollection services)
    {
        services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
        return services;
    }

    private static MapperConfiguration GetMapperConfiguration()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ExpenseMappingsProfile>();
            cfg.AddProfile<BalanceMappingsProfile>();
            cfg.AddProfile<RevenueMappingProfile>();
        });

        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}