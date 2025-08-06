using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Implementations;
using FinancialTracker.Services.AuthorizeApi.Presentation.Controllers.BaseControllers;
using FinancialTracker.Services.AuthorizeApi.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Controllers
{
    public class TokenController(
        ILogger<TokenController> logger,
        IUserRepository userRepository,
        IAuthTokenService tokenService,
        IAuthUseCasesFacade useCaseFacade)
        : TokenBaseController<TokenController>(logger)
    {
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<ITokenResponse>> Refresh(
            [FromBody]RefreshRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request for refresh token..");
            if (request == null || request.Jti == Guid.Empty)
                return BadRequest("Refresh token and JTI cannot are empty");

            var result = await useCaseFacade.RefreshAsync(
                userRepository, tokenService, request, cancellationToken);
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message!);
            _logger.LogInformation("refresh successfully");
            return Ok(result.Result);
        }

        [HttpPost("revoke")]
        [Authorize(Policy = nameof(Enum_AuthPolicy.CanAccess_AdminAndSuperUser))]
        public async Task<ActionResult> RevokeAllRefreshTokens(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Revoke all refresh tokens");
            var result = await useCaseFacade.RevokeAllRefreshTokensAsync(tokenService, cancellationToken);
            if (!result.IsSuccess)
                return UseCaseBadResultHandle(result.StatusCode, result.Message!);
            _logger.LogInformation("Revoked successfully");
            return NoContent();
        }
    }
}
