

using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Presentation.Helpers;
using Microsoft.OpenApi.Models;

namespace FinancialTracker.Services.AuthorizeApi.Presentation
{
    public static class DIPresentation
    {
        public static IServiceCollection AddPresentationLayer(
                this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            //services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            //    options.HttpsPort = 443;
            //});
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthorizeAPI", Version = "v1" });
                options.TagActionsBy(api => [api.GroupName]);
                options.DocInclusionPredicate((version, desc) => true);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Введите JWT токен в формате: Bearer {token}"
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            //turn off cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
                options.LoginPath = PathString.Empty;
                options.AccessDeniedPath = PathString.Empty;
            });

            //админу доступны функции обычного пользователя
            //суперпользователю кошелек ни к чему,
            //  это расширенный функционал админа по управлению пользователями и ролями
            services.AddAuthorization(options =>
            {
                List<Enum_BaseRoles> roles = [
                    Enum_BaseRoles.AdminAndUser,
                    Enum_BaseRoles.SUPERUSER,
                    Enum_BaseRoles.SuperUserAndAdmin,
                    Enum_BaseRoles.AllRoles,
                ];
                foreach (var role in roles)
                {
                    var policyType = Policy.CreatePolicy(role);
                    options.AddPolicy(policyType.Name,
                        policyBuilder => policyBuilder.RequireRole(policyType.Roles));
                }
            });
            return services;
        }
    }

}
