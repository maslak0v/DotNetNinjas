using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Implementations;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping;
using FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers;
using FinancialTracker.Services.AuthorizeApi.Presentation.Helpers;
using MessageBus.Shared.Contracts.Interfaces;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    public class AuthController(
        IAuthUseCasesFacade useCasesFacade,
        IUserRepository userRepository,
        IAuthTokenService tokenService,
        IMessagePublisher messagePublisher,
        ILogger<AuthController> logger) : AuthorizeBaseController<AuthController>(logger)
    {
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(
            [FromBody] UserRegisterRequest registerRequest,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registration of a new user...");
            var resultOperation = await useCasesFacade.UserRegisterAsync(
                userRepository, registerRequest, cancellationToken); 
            if (!resultOperation.IsSuccess)
                return UseCaseBadResultHandle(resultOperation.StatusCode, resultOperation.Message ?? string.Empty);
            
            var user = resultOperation.Result!;
            IUserCreated userCreatedMessage = user.ToUserCreatedMessage();

            _logger.LogInformation(
               $"Publish event [{userCreatedMessage.GetType()}]: user[{user.Id}] created");
            
            await messagePublisher.PublishAsync(userCreatedMessage, cancellationToken);

            return Created();
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ITokenResponse>> Login(
            [FromBody] UserLoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("try login ..");
            var result = await useCasesFacade.UserLoginAsync(
                userRepository, tokenService, request, cancellationToken);
            if (!result.IsSuccess)
            {
                _logger.LogWarning(result.Message);
                return Unauthorized();
            }
            _logger.LogInformation("login successfully");
            return Ok(result.Result);
        }

        [HttpPost("logout")]
        [Authorize(Policy = nameof(Enum_AuthPolicy.CanAccess_AllAuthUsers))]
        public async Task<ActionResult> Logout(CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"User [{userId}] logout");

            if (string.IsNullOrEmpty(userId))
                return BadRequest("user id is null or empty (claim \"sub\" not found)");

            var result = await useCasesFacade.UserLogoutAsync(userId, tokenService, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning(message: result.Message!);
                return UseCaseBadResultHandle(result.StatusCode, result.Message!);
            }
            return NoContent();
        }
    }
}
