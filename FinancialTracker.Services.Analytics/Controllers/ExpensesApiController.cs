using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;
using Primitives.Shared.DTOs;
using Primitives.Shared.Exceptions;

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
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        if (startDate > endDate)
            throw new BadRequestException("Error: Start date must be earlier than or equal to end date.");

        var expenses = await service.GetExpensesAsync(userId, startDate, endDate, cancellationToken);
        var response = new ResponseDto(mapper.Map<List<ExpenseResponseDto>>(expenses));
        return Ok(response);
    }

    [HttpGet("by-account")]
    public async Task<IActionResult> GetByAccountAsync([FromQuery] ExpensesRequestDto request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
            throw new BadRequestException("Error: Start date must be earlier than or equal to end date.");

        var expenses = await service.GetExpensesByAccountAsync(request, cancellationToken);
        var response = new ResponseDto(mapper.Map<List<ExpenseResponseDto>>(expenses));
        return Ok(response);
    }
}