using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/advice")]
public class AdviceApiController(IAdviceService adviceService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(
        [FromQuery] Guid userId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var response = new ResponseDto();
        
        try
        {
            var advices = await adviceService.GetAdviceAsync(userId, startDate, endDate);

            response.Result = mapper.Map<List<AdviceResponseDto>>(advices);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return Ok(response);
    }
}