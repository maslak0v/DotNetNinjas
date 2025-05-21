using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services.Analytics.Models;

[Index(nameof(Guid), IsUnique = true)]
public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public Guid Guid { get; set; }
    public string Name { get; set; } = string.Empty;
}