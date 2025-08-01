using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/expenses")]
[ApiExplorerSettings(GroupName = "Расходы")]
public class ExpensesApiController(IExpensesService service,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(
        [FromQuery] Guid userId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var response = new ResponseDto();
        if (startDate > endDate)
        {
            response.IsSuccess = false;
            response.Message = "Error: Start date must be earlier than or equal to end date.";
            return BadRequest(response);
        }
        try
        {
            var expenses = await service.GetExpensesAsync(userId, startDate, endDate);

            response.Result = mapper.Map<List<ExpenseResponseDto>>(expenses);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return StatusCode(500, response);
        }
        return Ok(response);
    }

    [HttpGet("by-account")]
    public async Task<IActionResult> GetByAccountAsync([FromQuery] ExpensesRequestDto request)
    {
        var response = new ResponseDto();
        if (request.StartDate > request.EndDate)
        {
            response.IsSuccess = false;
            response.Message = "Error: Start date must be earlier than or equal to end date.";
            return BadRequest(response);
        }
        try
        {
            var expenses = await service.GetExpensesByAccountAsync(request);
            response.Result = mapper.Map<List<ExpenseResponseDto>>(expenses);
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