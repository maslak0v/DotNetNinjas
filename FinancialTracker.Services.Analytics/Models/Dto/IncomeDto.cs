namespace FinancialTracker.Services.Analytics.Models.Dto;

public class IncomeDto
{
    public required Guid IncomeId { get; init; }
    public required Guid UserId { get; init; }
    public required Guid AccountId { get; init; }
    public string? Category { get; init; }
    public required DateTime IncomeTime { get; init; }
    public required decimal Amount { get; init; }
}