using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Helpers
{
    /// <summary>
    ///настройки авторизации для сваггера(кнопка для jwt, проверка доступа по ролям)
    /// </summary>
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Получаем атрибуты контроллера и метода
            var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttribute<AuthorizeAttribute>() != null
                               || context.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() != null;

            var hasAllowAnonymous = context.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() != null;

            if (hasAuthorize && !hasAllowAnonymous)
            {
                // Добавляем в Swagger информацию о необходимости авторизации
                operation.Security ??= new List<OpenApiSecurityRequirement>();

                operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                   Array.Empty<string>()
                }
            });
            }
        }
    }
}
