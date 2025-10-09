namespace FinancialTracker.Frontend.Services
{
	public class WalletService
	{
		private readonly HttpClientFactory _httpClientFactory;
		public WalletService(HttpClientFactory httpClientFactory, BrowserStorage browserStorage)
		{
			_httpClientFactory = httpClientFactory;
		}

	}
}
