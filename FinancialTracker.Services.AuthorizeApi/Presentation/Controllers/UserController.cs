using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    public class UserController(
        IAuthUseCasesFacade useCasesFacade,
        ILogger<UserController> logger): UserControllerBase<UserController>(logger)
    {

        /// <summary>
        /// Получить всех пользователей 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //todo: policy
        //[Authorize(Roles = $"{nameof(Enum_BaseRoles.ADMIN)}, {nameof(Enum_BaseRoles.SUPERUSER)}")]
        public async Task<ActionResult> GetAllUsers()
        {
            _logger.LogInformation("Get all users");
            var result = await useCasesFacade.GetAllUsersAsync();
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message ?? string.Empty);
            _logger.LogInformation($"found {result.Result!.Count} elements");
            return Ok(result.Result);
        }
    }
}
