namespace FinancialTracker.Services.Analytics.Models.Dto;

public sealed record CategoryReportItemDto(
    string Category,
    decimal TotalAmount,
    int TransactionCount);