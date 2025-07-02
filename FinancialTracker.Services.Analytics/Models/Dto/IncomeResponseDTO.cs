using Wallet.Domain.Entities;

namespace FinancialTracker.Services.Analytics.Models.Dto;

public class IncomeResponseDTO
{
    public DateTime ExpenseTime { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
    public Category Category { get; set; }
}
