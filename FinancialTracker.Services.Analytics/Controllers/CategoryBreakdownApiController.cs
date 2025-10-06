using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;
using Primitives.Shared.Exceptions;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/category-breakdown")]
[ApiExplorerSettings(GroupName = "Сводка по категориям")]
public class CategoryBreakdownApiController(
    ICategoryReportService service,
    IMapper mapper) : ControllerBase
{
    [HttpGet("category-report")]
    public async Task<IActionResult> GetCategoryReport(
        [FromQuery] CategoryReportRequestDto request,
        CancellationToken cancellationToken)
    {
        if(request.StartDate > request.EndDate)
            throw new BadRequestException("Дата конца периода должна быть больше даты начала периода.");

        var report = await service.GetCategoryReportAsync(
            request.UserId, request.StartDate, request.EndDate, cancellationToken);

        return Ok(report);
    }
}