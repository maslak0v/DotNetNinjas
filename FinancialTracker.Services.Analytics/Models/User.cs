using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Services.Analytics.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
}