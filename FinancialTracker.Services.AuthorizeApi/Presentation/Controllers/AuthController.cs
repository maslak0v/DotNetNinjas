using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Presentation.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IUseCasesFacade useCasesFacade,
        ILogger<AuthController> logger) : ControllerBase
    {
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]UserRegisterRequest registerRequest)
        {

            logger.LogInformation("Registration of a new user...");
            var result = await useCasesFacade.UserRegisterAsync(registerRequest);
            string message = result.Message ?? string.Empty;
            if (!result.IsSuccess)
            {
                logger.LogWarning(message);
                return result.StatusCode switch
                {
                    Enum_StatusCode.BAD_REQUEST => BadRequest(message),
                    _ => Problem(
                        statusCode: (int)result.StatusCode,
                        detail: message)
                };

            }
            logger.LogInformation(message);
            return Ok(message);
        }
    }
}
