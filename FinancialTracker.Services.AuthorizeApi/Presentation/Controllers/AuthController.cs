
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IAuthUseCasesFacade useCasesFacade,
        IAuthTokenService tokenService,
        ILogger<AuthController> logger) : ControllerBase
    {
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterRequest registerRequest)
        {
            logger.LogInformation("Registration of a new user...");
            var result = await useCasesFacade.UserRegisterAsync(registerRequest); 
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message ?? string.Empty);

            logger.LogInformation("user created");
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
                return UseCaseBadResultHandle(result.StatusCode, result.Message ?? string.Empty);
            logger.LogInformation($"found {result.Result!.Count} elements");
            return Ok(result.Result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
        {
            logger.LogInformation("try login ..");
            var result = await useCasesFacade.UserLoginAsync(tokenService, request);
            if (!result.IsSuccess)
            {
                logger.LogWarning(result.Message);
                return Unauthorized();
            }
            logger.LogInformation("login successfully");
            return Ok(result.Result);
        }

        private ActionResult UseCaseBadResultHandle(Enum_StatusCode statusCode, string message)
        {
            logger.LogWarning(message);
            return statusCode switch
            {
                Enum_StatusCode.BAD_REQUEST => BadRequest(message),
                _ => Problem(
                    statusCode: (int)statusCode,
                    detail: message)
            };
        }
    }
}
