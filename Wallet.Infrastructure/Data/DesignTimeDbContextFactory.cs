using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Wallet.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WalletPostgresDbContext>
{
    public WalletPostgresDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WalletPostgresDbContext>();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Wallet.API"))
            .AddJsonFile("appsettings.json")
            .Build();
        
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("PGConnection"));

        return new WalletPostgresDbContext(optionsBuilder.Options);
    }
}