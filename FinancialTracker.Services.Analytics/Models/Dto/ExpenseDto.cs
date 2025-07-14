namespace FinancialTracker.Services.Analytics.Models.Dto;

public class ExpenseDto
{
    public required Guid ExpenseId { get; init; }
    public required Guid UserId { get; init; }
    public required Guid AccountId { get; init; }
    public string? Category { get; init; }
    public required DateTime ExpenseTime { get; init; }
    public required decimal Amount { get; init; }
}