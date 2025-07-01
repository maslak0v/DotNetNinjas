using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Mapping;
using FinancialTracker.Services.Analytics.Messaging;
using FinancialTracker.Services.Analytics.Models.Advice.Rules;
using FinancialTracker.Services.Analytics.Services;
using FinancialTracker.Services.Analytics.Services.Implementation;
using MessageBus.Shared;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics;

public static class Registrator
{
    public static void InstallDbConnection(this IServiceCollection serviceCollection)
    {
        var connectionString = Environment.GetEnvironmentVariable("ANALYTICS_DB_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Connection string for db is empty");

        serviceCollection.AddDbContext<AppDbContext>(options
            => options.UseNpgsql(connectionString));
    }
    
    public static async Task<IApplicationBuilder> ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        await using var dbContext =
            scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        return app;
    }
    
    public static IServiceCollection InstallServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IExpensesService, ExpensesService>()
            .AddScoped<IBalanceService, BalanceService>();
        return serviceCollection;
    }

    public static IServiceCollection InstallAdviceService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IAdviceRule, GoodHabitRule>();
        serviceCollection
            .AddScoped<IAdviceService, AdviceService>();
        
        return serviceCollection;
    }
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        return services.AddBusMessage_WithConsumersFromType(typeof(UserCreatedAnaliticsConsumer));
    }

    public static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IExpensesRepository, ExpensesRepository>()
            .AddScoped<IUserRepository,UserRepository>();

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
            cfg.AddProfile<AdviceMappingsProfile>();
        });

        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}