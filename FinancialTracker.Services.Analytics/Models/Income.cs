

namespace FinancialTracker.Services.Analytics.Models;

public class Income
{
    public Guid IncomeId { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public string? Category { get; set; }
    public string Currency { get; set; } = "RUB";
    public DateTime IncomeTime { get; set; }
    public decimal Amount { get; set; }
    public User User { get; set; } = null!;
}
