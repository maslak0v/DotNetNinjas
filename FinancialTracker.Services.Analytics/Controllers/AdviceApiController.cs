using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/advice")]
[ApiExplorerSettings(GroupName = "Operation History")]
public class AdviceApiController(IAdviceService adviceService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(
        [FromQuery] Guid userId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            return BadRequest("Дата конца периода должна быть больше даты начала периода.");
        
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
            return StatusCode(500, response);
        }
        return Ok(response);
    }
}