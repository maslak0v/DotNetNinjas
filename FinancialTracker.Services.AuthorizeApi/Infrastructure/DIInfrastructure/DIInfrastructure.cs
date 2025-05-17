using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Repositories;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Imlementation;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.DIInfrastructure
{
    public static class DIInfrastructure
    {
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var jwtSettings = configuration
                .GetSection(nameof(JwtSettings))
                .Get<JwtSettings>();
            var jwtkey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (string.IsNullOrWhiteSpace(jwtkey))
                throw new Exception("Jwtkey is not configured");
            //JwtSettings registration 
            //(To inject by IOptions<JwtSettings>)
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

            string? connectionString = Environment.GetEnvironmentVariable("AUTH_DB_CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Connection string for db is empty");

            //DbContext registration
            services.AddDbContext<AuthDbContext>(options
                => options.UseNpgsql(connectionString));

            //Set dbContext for identity
            services.AddIdentity<AuthUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<AuthDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtkey))
                };
            });

            services.AddScoped<ITokenService<AuthUser>, TokenServiceImpl>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
        {
            await using var scope = app.ApplicationServices.CreateAsyncScope();
            using var dbContext = 
                scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
