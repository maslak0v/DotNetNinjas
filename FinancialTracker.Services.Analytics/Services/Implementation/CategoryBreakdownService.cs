using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public sealed class CategoryReportService(
    IExpensesRepository expensesRepository,
    IIncomesRepository incomesRepository,
    IMapper mapper) : ICategoryReportService
{
    public async Task<CategoryReportResponseDto> GetCategoryReportAsync(
        Guid userId, DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        var startUtc = startDate.ToUniversalTime();
        var endUtc = endDate.ToUniversalTime();

        var expTask = expensesRepository.GetExpenseTotalByCategoryAsync(userId, (DateTime)startDate, endUtc, ct);
        var incTask = incomesRepository.GetIncomeTotalByCategoryAsync(userId, (DateTime)startDate, endUtc, ct);

        await Task.WhenAll(expTask, incTask);

        var expItems = mapper.Map<List<CategoryReportItemDto>>(expTask.Result);
        var incItems = mapper.Map<List<CategoryReportItemDto>>(incTask.Result);

        return new CategoryReportResponseDto(expItems, incItems);
    }
}
