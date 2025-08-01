
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Primitives.Shared.DTOs;
using Primitives.Shared.Exceptions;

namespace FinancialTracker.Services.Analytics.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(
            ILogger<ExceptionHandlingMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            CancellationToken cancellationToken = context.RequestAborted;
            try
            {
                await _next(context);
            }
            catch (GlobalException ex)
            {
                await ExceptionHandle(context, ex, ex.StatusCode, ex.Title, cancellationToken);
            }
            catch (Exception ex)
            {
                await ExceptionHandle(context, ex, 500, "Internal server error", cancellationToken);
            }
        }

        private async Task ExceptionHandle(
            HttpContext context,
            Exception ex,
            int statusCode,
            string title,
            CancellationToken cancellationToken)
        {
            _logger.LogError(ex, title);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new ResponseDto(
                Result: null,
                IsSuccess: false,
                Message: string.Empty,
                Error: new ErrorDto(
                    StatusCode: statusCode,
                    Title: title,
                    Details: ex.MessageWithInner(),
                    TraceId: context.TraceIdentifier)
            );
            await context.Response.WriteAsJsonAsync(response, cancellationToken);
        }
    }
}
