using Microsoft.EntityFrameworkCore;
using Wallet.API.Mappings;
using Wallet.Application.Services;
using Wallet.Domain.Interfaces;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.Data.Repositories;

namespace Wallet.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<WalletPostgresDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PGConnection")));
        
        services.AddAutoMapper(typeof(Startup));
        
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();
        
        services.AddControllers(); 
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        { 
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        
        app.UseEndpoints(endpoints => 
        {
            endpoints.MapControllers();
        });
    }
}