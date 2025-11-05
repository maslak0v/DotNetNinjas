namespace FinancialTracker.Services.Analytics.Models.Dto;

public sealed record CategoryReportResponseDto(
    List<CategoryReportItemDto> Expenses,
    List<CategoryReportItemDto> Incomes);
