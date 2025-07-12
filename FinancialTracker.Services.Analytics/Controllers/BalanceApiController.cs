using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/balance")]
[ApiExplorerSettings(GroupName = "Balance & Finances")]
public class BalanceApiController(IBalanceService service,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ApiExplorerSettings(GroupName = "User Balance")]
    public async Task<IActionResult> GetAsync(
        [FromQuery] Guid userId,
        [FromQuery] DateTime forDate)
    {
        var response = new ResponseDto();
        
        try
        {
            var balance = await service.GetBalanceAsync(userId, forDate);

            response.Result = mapper.Map<BalanceResponseDto>(balance);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return StatusCode(500, response);
        }
        return Ok(response);
    }
}