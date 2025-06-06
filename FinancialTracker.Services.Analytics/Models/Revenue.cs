using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.Services.Analytics.Models
{
    public class Revenue
    {
        [Key]
        public int RevenueId { get; set; }
        [ForeignKey("UserId")]
        public required User User { get; set; }
        [Required]
        public DateTime RevenueTime { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public Guid AccountId { get; set; }
        public string Currency { get; set; } = "RUB";
    }
}
