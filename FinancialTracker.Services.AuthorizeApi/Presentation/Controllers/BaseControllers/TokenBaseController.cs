using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers
{
    [Route("api/token")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Управление токенами")]
    public class TokenBaseController<TController>(ILogger<TController> logger)
        : AdvanceBaseController<TController>(logger)
    {}
}
