

using Microsoft.AspNetCore.Builder;
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
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = 443;
            });
            
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthorizeAPI", Version = "v1" });
                c.TagActionsBy(api => [api.GroupName]);
                c.DocInclusionPredicate((version, desc) => true);
            });

            //turn off cookie
            services.ConfigureApplicationCookie(options => {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
                options.LoginPath = PathString.Empty;
                options.AccessDeniedPath = PathString.Empty;
            });

            return services;
        }
    }

}
