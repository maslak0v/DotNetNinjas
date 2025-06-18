using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Mapping;
using FinancialTracker.Services.Analytics.Services;
using FinancialTracker.Services.Analytics.Services.Implementation;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics;

public static class Registrator
{
    public static IServiceCollection InstallServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IExpensesService, ExpensesService>()
            .AddScoped<IBalanceService, BalanceService>();
        return serviceCollection;
    }

    public static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IExpensesRepository, ExpensesRepository>();

        return serviceCollection;
    }
    
    public static async Task<IApplicationBuilder> ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        await using var dbContext =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        return app;
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
        });

        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}