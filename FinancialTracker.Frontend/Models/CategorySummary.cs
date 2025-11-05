using System.Text.Json.Serialization;

namespace FinancialTracker.Frontend.Models
{
	public class CategorySummary
	{
		[JsonPropertyName("categoryName")]
		public string CategoryName { get; set; }

		[JsonPropertyName("amount")]
		public decimal Amount { get; set; }

		[JsonPropertyName("percentage")]
		public decimal Percentage { get; set; }

		[JsonPropertyName("count")]
		public int Count { get; set; }
	}
}
