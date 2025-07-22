using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;
using Primitives.Shared.DTOs;
using Primitives.Shared.Exceptions;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/incomes")]
public class IncomeApiController(IIncomesService service,
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
            throw new BadRequestException("Start date must be earlier than or equal to end date.");
        
        var incomes = await service.GetIncomesAsync(userId, startDate, endDate, cancellationToken);
        var response = new ResponseDto(mapper.Map<List<IncomeResponseDTO>>(incomes));
        return Ok(response);
    }

    [HttpGet("by-account")]
    public async Task<IActionResult> GetByAccountAsync([FromQuery] IncomesRequestDTO request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
            throw new BadRequestException("Start date must be earlier than or equal to end date.");
        var incomes = await service.GetIncomesByAccountAsync(request, cancellationToken);
        var response = new ResponseDto(mapper.Map<List<IncomeResponseDTO>>(incomes));
        return Ok(response);
    }
}
