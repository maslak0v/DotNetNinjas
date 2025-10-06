namespace FinancialTracker.Services.Analytics.Models.Dto;

public sealed record CategoryReportRequestDto(
    Guid UserId,
    DateTime StartDate,
    DateTime EndDate);
