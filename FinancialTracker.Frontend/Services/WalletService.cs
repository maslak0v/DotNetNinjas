using FinancialTracker.Frontend.Models;
using System.Net.Http.Json;

namespace FinancialTracker.Frontend.Services
{
	public class WalletService
	{
		private readonly HttpClientFactory _httpClientFactory;
		public WalletService(HttpClientFactory httpClientFactory, BrowserStorage browserStorage)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<List<Account>> GetAllAccountsAsync(string userId)
		{
			using var client = await _httpClientFactory.CreateAuthenticatedWalletClient();

			return await client.GetFromJsonAsync<List<Account>>($"api/accounts/user/{userId}/all")
				?? new List<Account>();
		}
	}
}
