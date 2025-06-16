namespace FinancialTracker.Services.Analytics.Models.Dto
{
    public class IncomesResponseDto
    {
        public DateTime IncomeTime { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "RUB";
    }
}
