using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Middlewares
{
    /// <summary>
    /// Middleware for check a revoked access token
    /// </summary>
    /// <param name="next"></param>
    /// <param name="tokenService"></param>
    public class DenyRevokedJti(RequestDelegate next)
    {
       
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (IsAnonymousEndpoint(httpContext))
            {
                await next(httpContext);
                return;
            }
            if (!IsAuthenticated(httpContext))
            {
                await Send401ResponseAsync(httpContext, ErrorMessage.NotAuthenticated);
                return;
            }
            if (!await IsValidJti(httpContext))
            {
                await Send401ResponseAsync(httpContext, ErrorMessage.InvalidOrRevoked);
                return;
            }
            await next(httpContext);
        }

        private bool IsAuthenticated(HttpContext httpContext)
             => httpContext.User.Identity?.IsAuthenticated ?? false;

        private async Task Send401ResponseAsync(HttpContext httpContext, string message)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync(message);
        }
        private bool IsAnonymousEndpoint(HttpContext httpContext)
           => httpContext.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>() is not null;

        //not empty and not revoked = valid
        private async Task<bool> IsValidJti(HttpContext httpContext)
        {

            var jti = httpContext.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var tokenService = httpContext.RequestServices.GetRequiredService<IAuthTokenService>();
            return !string.IsNullOrEmpty(jti) && !await tokenService.IsRevokedRefreshTokenAsync(jti);
        }
        private static class ErrorMessage
        {
            public const string NotAuthenticated = "User not authenticated.";
            public const string InvalidOrRevoked = "Token is invalid or revoked.";
        }
    }
}
