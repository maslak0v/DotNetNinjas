namespace FinancialTracker.Services.Analytics.Models.Dto;

public class ExpenseResponseDto
{
    public DateTime ExpenseTime { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
}