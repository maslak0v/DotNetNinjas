using FinancialTracker.Services.Analytics.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/expenses")]
public class ExpensesApiController : ControllerBase
{
    private readonly ResponseDto _response;

    public ExpensesApiController()
    {
        _response = new ResponseDto();
    }
    
    [HttpGet]
    public IActionResult Get(
        [FromQuery] Guid userId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            _response.Result = "Hello";
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return Ok(_response);
    }
}