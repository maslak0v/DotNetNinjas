using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Services.Analytics.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Expense> Expenses { get; set; } = [];
}