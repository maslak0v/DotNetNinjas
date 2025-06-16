
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers
{

    [Route("api/user")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Управление пользователями")]
    public class UserBaseController<TController>(ILogger<TController> logger)
        : AdvanceBaseController<TController>(logger)
    {
    }
}
