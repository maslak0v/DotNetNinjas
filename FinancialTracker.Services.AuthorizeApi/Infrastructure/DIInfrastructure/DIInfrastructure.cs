using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.DataAccess;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Repositories;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Imlementation;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            //JwtSettings registration 
            //(To inject by IOptions<JwtSettings>)
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

            //DbContext registration
            services.AddDbContext<AuthDbContext>(options
                => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

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
                        Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            services.AddScoped<ITokenService<AuthUser>, TokenServiceImpl>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
