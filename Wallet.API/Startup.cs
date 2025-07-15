
using MessageBus.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Application.Interfaces.Services;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.Data.Repositories;
using Wallet.Infrastructure.Messaging;
using Wallet.Infrastructure.Services;

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
        var connectionString = Environment.GetEnvironmentVariable("WALLET_PG_CONNECTION_STRING");
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("The wallet's connection string is empty");
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

        services.AddBusMessage_WithConsumersFromType(typeof(UserCreatedWalletConsumer));
        
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