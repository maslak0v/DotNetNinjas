using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;
using Primitives.Shared.DTOs;
using Primitives.Shared.Exceptions;

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
        [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
    {
        if (startDate > endDate)
            throw new BadRequestException("Дата конца периода должна быть больше даты начала периода.");
        var advices = await adviceService.GetAdviceAsync(userId, startDate, endDate, cancellationToken);
        var response = new ResponseDto(mapper.Map<List<AdviceResponseDto>>(advices));

        return Ok(response);
    }
}