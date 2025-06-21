using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Wallet.Application.Interfaces;
using Wallet.Application.Services;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.Data.Interfaces;
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
        var envPath = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "SettingsData.env"
        );
        envPath = Path.GetFullPath(envPath);
        
        DotNetEnv.Env.Load(envPath);
        var connectionString = Environment.GetEnvironmentVariable("PG_CONNECTION_STRING");

        services.AddDbContext<WalletPostgresDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddAutoMapper(typeof(Startup));
        
        // Репозитории 
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ITransactionTagRepository, TransactionTagRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITransactionRepository , TransactionRepository>();
        
        //Сервисы
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ITransactionService, TransactionService>();
        
        services.AddControllers(); 
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet API", Version = "v1" });
            c.TagActionsBy(api => new[] { api.GroupName });
            c.DocInclusionPredicate((version, desc) => true);
        });
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