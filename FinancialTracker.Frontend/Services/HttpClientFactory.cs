using System.Net.Http.Headers;

namespace FinancialTracker.Frontend.Services
{
	public class HttpClientFactory
	{
		private readonly BrowserStorage _browserStorage;

		public HttpClientFactory(BrowserStorage browserStorage)
		{
			_browserStorage = browserStorage;
		}

		public HttpClient CreateAuthClient()
		{
			return new HttpClient{ BaseAddress = new Uri("http://localhost:5010/") };
		}

		public HttpClient CreateWalletClient()
		{
			return new HttpClient { BaseAddress = new Uri("http://localhost:5020/") };
		}

		public HttpClient CreateAnalyticsClient()
		{
			return new HttpClient { BaseAddress = new Uri("http://localhost:5030/") };
		}

		public async Task<HttpClient> CreateAuthenticatedAuthClient()
		{
			var client = CreateAuthClient();
			await AddAuthorizationHeader(client);
			return client;
		}

		public async Task<HttpClient> CreateAuthenticatedWalletClient()
		{
			var client = CreateWalletClient();
			await AddAuthorizationHeader(client);
			return client;
		}

		public async Task<HttpClient> CreateAuthenticatedAnalyticsClient()
		{
			var client = CreateAnalyticsClient();
			await AddAuthorizationHeader(client);
			return client;
		}

		private async Task AddAuthorizationHeader(HttpClient client)
		{
			var token = await _browserStorage.GetAsync("accessToken");
			if (!string.IsNullOrEmpty(token))
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);
			}
		}
	}
}
