using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.Services.Analytics.Models;

public class Expense
{
    [Key]
    public int ExpenseId { get; set; }
    [ForeignKey("UserId")]
    public required User User { get; set; }
    [Required]
    public DateTime ExpenseTime { get; set; }
    [Required]
    public decimal Amount { get; set; }
    
    // Новые поля для счета и валюты
    public Guid AccountId { get; set; }
    public string Currency { get; set; } = "RUB";
}