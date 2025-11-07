using System.Text.Json.Serialization;

namespace FinancialTracker.Frontend.Models
{
	public class AnalyticsResponse<T>
	{
		[JsonPropertyName("result")]
		public T Result { get; set; }

		[JsonPropertyName("isSuccess")]
		public bool IsSuccess { get; set; }

		[JsonPropertyName("message")]
		public string Message { get; set; }

		[JsonPropertyName("error")]
		public string Error { get; set; }
	}
}
