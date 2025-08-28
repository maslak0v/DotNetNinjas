using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers;
using FinancialTracker.Services.AuthorizeApi.Presentation.Helpers;
using MessageBus.Shared.Contracts.Implementations;
using MessageBus.Shared.Contracts.Interfaces;
using MessageBus.Shared.Publishers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    public class UserController(
        IAuthUseCasesFacade useCasesFacade,
        IUserRepository userRepository,
        IMessagePublisher messagePublisher,
        ILogger<UserController> logger): UserBaseController<UserController>(logger)
    {

        /// <summary>
        /// Получить всех пользователей 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = nameof(Enum_AuthPolicy.CanAccess_AdminAndSuperUser))]
        public async Task<ActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all users");
            var result = await useCasesFacade.GetAllUsersAsync(userRepository, cancellationToken);
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message ?? string.Empty);
            _logger.LogInformation($"found {result.Result!.Count} elements");
            return Ok(result.Result);
        }

        /// <summary>
        /// Удалить свою учетку
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = nameof(Enum_AuthPolicy.CanAccess_AdminAndUser))]
        public async Task<ActionResult> Delete(CancellationToken cancellationToken)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var result = await useCasesFacade.DeleteAsync(userId, userRepository, cancellationToken);
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message ?? string.Empty);

            await PublishDeleteEventAsync (userId, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Удалить другого пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [Authorize(Policy = nameof(Enum_AuthPolicy.CanAccess_AdminAndSuperUser))]
        public async Task<ActionResult> Delete(Guid userId, CancellationToken cancellationToken)
        {
            string userIdStr = userId.ToString();

            var result = await useCasesFacade.DeleteAsync(userIdStr, userRepository, cancellationToken);
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message ?? string.Empty);

            await PublishDeleteEventAsync(userIdStr, cancellationToken);

            return NoContent();
        }


        private async Task PublishDeleteEventAsync(string userId, CancellationToken cancellationToken)
        {

            //Event deleting
            IUserDeleted userDeletedMessage = new UserDeletedMessage(
                Guid.Parse(userId),
                Guid.CreateVersion7(),
                DateTime.UtcNow);
            _logger.LogInformation(
                $"Publish event [{userDeletedMessage.GetType()}]: user[{userId}] deleted");

            await messagePublisher.PublishAsync(userDeletedMessage, cancellationToken);
        }
    }
}
