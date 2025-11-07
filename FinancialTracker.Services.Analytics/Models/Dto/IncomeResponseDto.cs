
namespace FinancialTracker.Services.Analytics.Models.Dto;

public class IncomeResponseDto
{
    public DateTime IncomeTime { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
    public string? Category { get; set; }
}
