using System.Text.Json.Serialization;

namespace FinancialTracker.Frontend.Models
{
	public class Balance
	{
		[JsonPropertyName("amount")]
		public decimal Amount { get; set; }

		[JsonPropertyName("currency")]
		public string Currency { get; set; }
	}
}
