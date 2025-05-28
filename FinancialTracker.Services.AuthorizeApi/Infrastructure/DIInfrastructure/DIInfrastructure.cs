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
            string? jwtkey = GetSecretKey();
            JwtSettings? jwtSettings = JwtSettingsRegistration(services, configuration);

            DbContextRegistration(services);
            SetDbContextToIdentity(services);

            Registration_AuthenticationJwt(services, jwtkey, jwtSettings);

            AddScopedServices(services);

            return services;
        }


        #region private
        private static void AddScopedServices(IServiceCollection services)
        {
            services.AddScoped<ITokenService<AuthUser>, TokenServiceImpl>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
        private static void Registration_AuthenticationJwt(IServiceCollection services, string jwtkey, JwtSettings? jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidateAudience = false,
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtkey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        /// <summary>
        /// Get key from some store
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string GetSecretKey()
        {
            var jwtkey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (string.IsNullOrWhiteSpace(jwtkey))
                throw new Exception("Jwtkey is not configured");
            return jwtkey;
        }

        private static void SetDbContextToIdentity(IServiceCollection services)
        {
            services.AddIdentity<AuthUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<AuthDbContext>()
              .AddDefaultTokenProviders();
        }

        private static void DbContextRegistration(IServiceCollection services)
        {
            //check connection string
            string? connectionString = Environment.GetEnvironmentVariable("AUTH_DB_CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Connection string for db is empty");

            //DbContext registration
            services.AddDbContext<AuthDbContext>(options
                => options.UseNpgsql(connectionString));
        }

        private static JwtSettings? JwtSettingsRegistration(IServiceCollection services, IConfiguration configuration)
        {
            //JwtSettings registration 
            //(To inject by IOptions<JwtSettings>)
            var sectionJwt = configuration
                .GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(sectionJwt);
            var jwtSettings = sectionJwt.Get<JwtSettings>();
            return jwtSettings;
        }

        public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
        {
            await using var scope = app.ApplicationServices.CreateAsyncScope();
            using var dbContext = 
                scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            await dbContext.Database.MigrateAsync();
        }
        #endregion
    }
}
