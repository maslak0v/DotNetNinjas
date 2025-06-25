using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Implementations;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Mapping;
using FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers;
using MessageBus.Shared.Contracts.Implementations;
using MessageBus.Shared.Contracts.Interfaces;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    public class AuthController(
        IAuthUseCasesFacade useCasesFacade,
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
        public async Task<ActionResult> Register([FromBody] UserRegisterRequest registerRequest)
        {
            _logger.LogInformation("Registration of a new user...");
            var resultOperation = await useCasesFacade.UserRegisterAsync(registerRequest); 
            if (!resultOperation.IsSuccess)
                return UseCaseBadResultHandle(resultOperation.StatusCode, resultOperation.Message ?? string.Empty);
            
            var user = resultOperation.Result!;
            IUserCreated userCreatedMessage = user.ToUserCreatedMessage();

            _logger.LogInformation(
               $"Publish event [{userCreatedMessage.GetType()}]: user[{user.Id}] created");
            
            await messagePublisher.PublishAsync(userCreatedMessage);

            return Created();
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ITokenResponse>> Login([FromBody] UserLoginRequest request)
        {
            _logger.LogInformation("try login ..");
            var result = await useCasesFacade.UserLoginAsync(tokenService, request);
            if (!result.IsSuccess)
            {
                _logger.LogWarning(result.Message);
                return Unauthorized();
            }
            _logger.LogInformation("login successfully");
            return Ok(result.Result);
        }

    }
}
