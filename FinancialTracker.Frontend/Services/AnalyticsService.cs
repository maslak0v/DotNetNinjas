namespace FinancialTracker.Frontend.Services
{
	public class AnalyticsService
	{
		private readonly HttpClientFactory _httpClientFactory;
		public AnalyticsService(HttpClientFactory httpClientFactory, BrowserStorage browserStorage)
		{
			_httpClientFactory = httpClientFactory;
		}
	}
}
