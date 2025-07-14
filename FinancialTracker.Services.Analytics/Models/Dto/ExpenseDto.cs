namespace FinancialTracker.Services.Analytics.Models.Dto;

public class ExpenseDto
{
    public Guid ExpenseId { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public string? Category { get; set; }
    public DateTime ExpenseTime { get; set; }
    public decimal Amount { get; set; }
}