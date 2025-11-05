using System.Text.Json.Serialization;

namespace FinancialTracker.Frontend.Models
{
	public class Advice
	{
		[JsonPropertyName("title")]
		public string Title { get; set; }
		[JsonPropertyName("message")]
		public string Message { get; set; }
	}
}
