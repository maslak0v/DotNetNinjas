using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts;
using FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    public class TokenController(
        ILogger<TokenController> logger,
        IAuthTokenService tokenService,
        IAuthUseCasesFacade useCasefacade)
        : AuthorizeBaseController<TokenController>(logger)
    {
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<ITokenResponse>> Refresh([FromBody]RefreshRequest request)
        {
            _logger.LogInformation("Request for refresh token..");
            if (request == null || request.Jti == Guid.Empty)
                return BadRequest("Refresh token and JTI cannot are empty");

            var result = await useCasefacade.Refresh(tokenService, request);
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message!);
            _logger.LogInformation("refresh successfully");
            return Ok(result.Result);
        }
    }
}
