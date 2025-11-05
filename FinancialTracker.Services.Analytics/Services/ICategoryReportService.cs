using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services;

public interface ICategoryReportService
{
    Task<CategoryReportResponseDto> GetCategoryReportAsync(
        Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken);
}
