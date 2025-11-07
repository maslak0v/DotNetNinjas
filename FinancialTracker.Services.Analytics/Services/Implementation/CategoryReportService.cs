using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public sealed class CategoryReportService(
    IServiceProvider serviceProvider,
    IMapper mapper) : ICategoryReportService
{
    public async Task<CategoryReportResponseDto> GetCategoryReportAsync(
        Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var startUtc = startDate.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(startDate, DateTimeKind.Utc)
            : startDate.ToUniversalTime();
        var endUtc = endDate.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(endDate, DateTimeKind.Utc)
            : endDate.ToUniversalTime();

        await using var expenseScope = serviceProvider.CreateAsyncScope();
        await using var incomeScope = serviceProvider.CreateAsyncScope();

        var expenseRepo = expenseScope.ServiceProvider.GetRequiredService<IExpensesRepository>();
        var incomeRepo = incomeScope.ServiceProvider.GetRequiredService<IIncomesRepository>();

        var expenseTask = expenseRepo.GetExpenseTotalByCategoryAsync(userId, startUtc, endUtc, cancellationToken);
        var incomeTask = incomeRepo.GetIncomeTotalByCategoryAsync(userId, startUtc, endUtc, cancellationToken);

        await Task.WhenAll(expenseTask, incomeTask);

        return new CategoryReportResponseDto(
            mapper.Map<List<CategoryReportItemDto>>(expenseTask.Result),
            mapper.Map<List<CategoryReportItemDto>>(incomeTask.Result));
    }
}
