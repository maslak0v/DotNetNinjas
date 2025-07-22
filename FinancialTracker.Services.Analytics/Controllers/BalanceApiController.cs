using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;
using Primitives.Shared.DTOs;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/balance")]
public class BalanceApiController(IBalanceService service,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(
        [FromQuery] Guid userId,
        [FromQuery] DateTime forDate,
        CancellationToken cancellationToken)
    {
        var balance = await service.GetBalanceAsync(userId, forDate, cancellationToken);
        var response = new ResponseDto(mapper.Map<BalanceResponseDto>(balance));

        return Ok(response);
    }
}