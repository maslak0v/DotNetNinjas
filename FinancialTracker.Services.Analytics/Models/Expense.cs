

namespace FinancialTracker.Services.Analytics.Models;

public class Expense
{
    public Guid ExpenseId { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public string? Category { get; set; }
    public string Currency { get; set; } = "RUB";
    public DateTime ExpenseTime { get; set; }
    public decimal Amount { get; set; }
    // Навигация
    public User User { get; set; } = null!;
}