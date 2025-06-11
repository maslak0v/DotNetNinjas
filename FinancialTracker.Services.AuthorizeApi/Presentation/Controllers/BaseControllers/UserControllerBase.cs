
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers
{

    [Route("api/user")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Управление пользователями")]
    public class UserControllerBase<TController>(ILogger<TController> logger)
        : ControllerBaseAdvance<TController>(logger)
    {
    }
}
