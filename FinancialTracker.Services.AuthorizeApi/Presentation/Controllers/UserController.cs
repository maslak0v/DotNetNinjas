using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers;
using FinancialTracker.Services.AuthorizeApi.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    public class UserController(
        IAuthUseCasesFacade useCasesFacade,
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
            var result = await useCasesFacade.GetAllUsersAsync(cancellationToken);
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message ?? string.Empty);
            _logger.LogInformation($"found {result.Result!.Count} elements");
            return Ok(result.Result);
        }
    }
}
