using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Wallet.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WalletDbContext>
{
    public WalletDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WalletDbContext>();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Wallet.API"))
            .AddJsonFile("appsettings.json")
            .Build();
        
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("PGConnection"));

        return new WalletDbContext(optionsBuilder.Options);
    }
}