using MassTransit;
using MessageBus.Shared.Publishers.Imlementations;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBus.Shared
{
    public static class BusConfigurator
    {
        /// <summary>
        /// simple bus registry
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureConsumers"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusMessaging(
            this IServiceCollection services,
            Action<IBusRegistrationConfigurator>? configure = null)
        {
            var credentialData = GetCredentialsData();

            services.AddMassTransit(cfg =>
            {
                configure?.Invoke(cfg);

                
                cfg.UsingRabbitMq((context, busConfigurator) =>
                {
                    busConfigurator.Host(credentialData.host, "/", config =>
                    {
                        config.Username(credentialData.user);
                        config.Password(credentialData.password);
                    });

                    busConfigurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        public static IServiceCollection AddDefaultPublisher(
            this IServiceCollection services)
            => services.AddScoped<IMessagePublisher, MessagePublisher>();


        /// <summary>
        /// registration of consumers when they are in other library
        /// </summary>
        /// <param name="services"></param>
        /// <param name="consumers"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusMessage_WithConsumersFromType(
            this IServiceCollection services,
            params Type[] consumerTypes)
        {
            return services.AddBusMessaging(cfg =>
            {
                foreach (var consumerType in consumerTypes)
                    cfg.AddConsumers(consumerType.Assembly);
            });
        }

        private static (string user, string password, string host) GetCredentialsData()
        {
            var user = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER");
            var password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS");
            var host = Environment.GetEnvironmentVariable("RABBITMQ__HOST");
            if (string.IsNullOrEmpty(user) ||
               string.IsNullOrEmpty(password) ||
               string.IsNullOrEmpty(host))
                throw new Exception("Not found settings for rabbitMq (check user, password and host)");
            return (user, password, host);
        }
    }
}
