using System.Text.Json.Serialization;

namespace FinancialTracker.Frontend.Models
{
	public class Expense
	{
		[JsonPropertyName("amount")]
		public decimal Amount { get; set; }

		[JsonPropertyName("currency")]
		public string Currency { get; set; }

		[JsonPropertyName("category")]
		public string Category { get; set; }

		[JsonPropertyName("expense-time")]
		public DateTime ExpenseTime { get; set; }
	}
}
