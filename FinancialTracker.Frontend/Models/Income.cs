using System.Text.Json.Serialization;

namespace FinancialTracker.Frontend.Models
{
	public class Income
	{
		[JsonPropertyName("incomeId")]
		public required Guid IncomeId { get; set; }
		[JsonPropertyName("userId")]
		public required Guid UserId { get; set; }
		[JsonPropertyName("accountId")]
		public required Guid AccountId { get; set; }
		[JsonPropertyName("category")]
		public string? Category { get; set; }
		[JsonPropertyName("incomeTime")]
		public required DateTime IncomeTime { get; set; }
		[JsonPropertyName("amount")]
		public required decimal Amount { get; set; }
	}
}
