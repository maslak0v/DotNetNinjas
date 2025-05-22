using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IAuthUseCasesFacade useCasesFacade,
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
            logger.LogInformation(string.IsNullOrEmpty(message)? "user created" : message);
            return Created();
        }

        /// <summary>
        /// Получить всех пользователей 
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers()
        {
            logger.LogInformation("Get all users");
            var result = await useCasesFacade.GetAllUsersAsync();
            if (!result.IsSuccess)
            {
                logger.LogError(result.Message);
                return Problem(
                    detail: result.Message,
                    statusCode: (int)result.status);
            }
            logger.LogInformation($"found {result.Result!.Count} elements");
            return Ok(result);
        }
    }
}
