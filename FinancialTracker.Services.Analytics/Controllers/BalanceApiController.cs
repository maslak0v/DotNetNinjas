using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/balance")]
public class BalanceApiController(IBalanceService service,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult Get(
        [FromQuery] Guid userId,
        [FromQuery] DateTime forDate)
    {
        var response = new ResponseDto();
        
        try
        {
            var balance = service.GetBalance(userId, forDate);

            response.Result = mapper.Map<BalanceResponseDto>(balance);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return Ok(response);
    }
}