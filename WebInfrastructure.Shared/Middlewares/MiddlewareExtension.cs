using FinancialTracker.Services.Analytics.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace WebInfrastructure.Shared.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UsegGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
