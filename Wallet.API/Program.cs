using Microsoft.EntityFrameworkCore;
using Wallet.Infrastructure.Data;

namespace Wallet.API
{
    public class Program
    {
        private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(
                (hostingContext, builder) =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.Development.json", false, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();
                })
            .ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(
                        options => { options.AddServerHeader = false; });
                });

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Применяем миграции при запуске
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<WalletPostgresDbContext>();
                context.Database.Migrate();
            }
            
            host.Run();
        }
    }
}