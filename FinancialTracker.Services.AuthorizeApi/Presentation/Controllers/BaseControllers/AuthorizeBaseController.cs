using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers
{
    [Route("api/authorize")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Авторизация")]
    public class AuthorizeBaseController<TController>(ILogger<TController> logger)
        : ControllerBaseAdvance<TController>(logger)
    {
    }
}
