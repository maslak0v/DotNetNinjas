using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Imlementation;
using MassTransit;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.DIInfrastructure
{
    public static class BusIntegrator
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            string? user = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER");
            string? password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS");
            string? host = Environment.GetEnvironmentVariable("RABBITMQ__HOST");

            if (string.IsNullOrEmpty(user) ||
               string.IsNullOrEmpty(password) ||
               string.IsNullOrEmpty(host))
                throw new Exception("Not found settings for rabbitMq (check user, password and host)");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, busConfigurator) =>
                {
                    busConfigurator.Host(host, "/", config =>
                    {
                        config.Username(user);
                        config.Password(password);
                    });
                    busConfigurator.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
