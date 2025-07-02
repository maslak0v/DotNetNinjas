

namespace FinancialTracker.Services.Analytics.Models;

public class Income
{
    public int IncomeId { get; set; }
    public DateTime IncomeTime { get; set; }
    public decimal Amount { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string? Category { get; set; }
    public string Currency { get; set; } = "RUB";
    public User User { get; set; } = null!;
}
