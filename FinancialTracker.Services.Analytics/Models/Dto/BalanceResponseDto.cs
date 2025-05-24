namespace FinancialTracker.Services.Analytics.Models.Dto;

public class BalanceResponseDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
}