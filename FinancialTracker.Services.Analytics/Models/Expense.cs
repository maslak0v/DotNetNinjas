

namespace FinancialTracker.Services.Analytics.Models;

public class Expense
{
    public int ExpenseId { get; set; }
    public DateTime ExpenseTime { get; set; }
    public decimal Amount { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string Currency { get; set; } = "RUB";

    // Навигация
    public User User { get; set; } = null!;
}