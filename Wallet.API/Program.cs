namespace Wallet.API;

public class Program
{
    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(
            (hostingContext, builder) =>
            {
                builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json", false, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true,
                        true)
                    .AddEnvironmentVariables();
            
                builder.AddEnvironmentVariables();
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
        CreateHostBuilder(args).Build().Run();
    }
}