using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers
{
    public class AdvanceBaseController<TController>(ILogger<TController> logger)
        : ControllerBase
    {
        protected readonly ILogger<TController> _logger = logger;
        protected ActionResult UseCaseBadResultHandle(Enum_StatusCode statusCode, string message)
        {
            _logger.LogWarning(message);
            return statusCode switch
            {
                Enum_StatusCode.BAD_REQUEST => BadRequest(message),
                Enum_StatusCode.NOT_FOUND => NotFound(),
                Enum_StatusCode.NO_CONTENT => NoContent(),
                _ => Problem(
                    statusCode: (int)statusCode,
                    detail: message)
            };
        }
    }
}
