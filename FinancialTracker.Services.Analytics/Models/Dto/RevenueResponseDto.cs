namespace FinancialTracker.Services.Analytics.Models.Dto
{
    public class RevenueResponseDto
    {
        public DateTime RevenueTime { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RUB";
    }
}
