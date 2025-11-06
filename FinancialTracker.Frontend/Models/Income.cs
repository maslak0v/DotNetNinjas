using System.Text.Json.Serialization;

namespace FinancialTracker.Frontend.Models
{
	public class Income
	{
		[JsonPropertyName("category")]
		public string? Category { get; set; }
		[JsonPropertyName("amount")]
		public required decimal Amount { get; set; }
		[JsonPropertyName("currency")]
		public required string Currency { get; set; }
		[JsonPropertyName("incomeTime")]
		public required DateTime IncomeTime { get; set; }
	}
}
