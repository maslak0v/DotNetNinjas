using FinancialTracker.Frontend.Models;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace FinancialTracker.Frontend.Services
{
	public class AnalyticsService
	{
		private readonly HttpClientFactory _httpClientFactory;
		public AnalyticsService(HttpClientFactory httpClientFactory, BrowserStorage browserStorage)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<AnalyticsResponse<Balance>> GetBalanceAsync(string userId, DateTime forDate)
		{
			try
			{
				using var client = await _httpClientFactory.CreateAuthenticatedAnalyticsClient();

				string dateString = forDate.ToString("yyyy-MM-dd");
				string url = $"api/balance?userId={userId}&forDate={dateString}";

				var response = await client.GetFromJsonAsync<AnalyticsResponse<Balance>>(url);
				return response ?? CreateErrorResponse<Balance>("Пустой ответ от сервера");
			}
			catch (Exception ex)
			{
				return CreateErrorResponse<Balance>($"Ошибка: {ex.Message}");
			}
		}

		public async Task<AnalyticsResponse<List<Income>>> GetIncomesAsync(string userId, DateTime startDate, DateTime endDate)
		{
			try
			{
				using var client = await _httpClientFactory.CreateAuthenticatedAnalyticsClient();

				string start = startDate.ToString("yyyy-MM-dd");
				string end = endDate.ToString("yyyy-MM-dd");
				string url = $"api/incomes?userId={userId}&startDate={start}&endDate={end}";

				var response = await client.GetFromJsonAsync<AnalyticsResponse<List<Income>>>(url);
				return response ?? CreateErrorResponse<List<Income>>("Пустой ответ от сервера");
			}
			catch (Exception ex)
			{
				return CreateErrorResponse<List<Income>>($"Ошибка получения доходов: {ex.Message}");
			}
		}

		public async Task<AnalyticsResponse<List<Income>>> GetIncomesByAccountAsync(string userId, DateTime startDate, DateTime endDate, string accountId)
		{
			try
			{
				using var client = await _httpClientFactory.CreateAuthenticatedAnalyticsClient();

				string start = startDate.ToString("yyyy-MM-dd");
				string end = endDate.ToString("yyyy-MM-dd");
				string url = $"api/incomes/by-account?userId={userId}&startDate={start}&endDate={end}&accountId={accountId}";

				var response = await client.GetFromJsonAsync<AnalyticsResponse<List<Income>>>(url);
				return response ?? CreateErrorResponse<List<Income>>("Пустой ответ от сервера");
			}
			catch (Exception ex)
			{
				return CreateErrorResponse<List<Income>>($"Ошибка получения доходов по счету: {ex.Message}");
			}
		}

		public async Task<AnalyticsResponse<List<Expense>>> GetExpensesAsync(string userId, DateTime startDate, DateTime endDate)
		{
			try
			{
				using var client = await _httpClientFactory.CreateAuthenticatedAnalyticsClient();

				string start = startDate.ToString("yyyy-MM-dd");
				string end = endDate.ToString("yyyy-MM-dd");
				string url = $"api/expenses?userId={userId}&startDate={start}&endDate={end}";

				var response = await client.GetFromJsonAsync<AnalyticsResponse<List<Expense>>>(url);
				return response ?? CreateErrorResponse<List<Expense>>("Пустой ответ от сервера");
			}
			catch (Exception ex)
			{
				return CreateErrorResponse<List<Expense>>($"Ошибка получения расходов: {ex.Message}");
			}
		}

		public async Task<AnalyticsResponse<List<Expense>>> GetExpensesByAccountAsync(string userId, DateTime startDate, DateTime endDate, string accountId)
		{
			try
			{
				using var client = await _httpClientFactory.CreateAuthenticatedAnalyticsClient();

				string start = startDate.ToString("yyyy-MM-dd");
				string end = endDate.ToString("yyyy-MM-dd");
				string url = $"api/expenses/by-account?userId={userId}&startDate={start}&endDate={end}&accountId={accountId}";

				var response = await client.GetFromJsonAsync<AnalyticsResponse<List<Expense>>>(url);
				return response ?? CreateErrorResponse<List<Expense>>("Пустой ответ от сервера");
			}
			catch (Exception ex)
			{
				return CreateErrorResponse<List<Expense>>($"Ошибка получения расходов по счету: {ex.Message}");
			}
		}

		public async Task<AnalyticsResponse<List<Advice>>> GetAdviceAsync(string userId, DateTime startDate, DateTime endDate)
		{
			try
			{
				using var client = await _httpClientFactory.CreateAuthenticatedAnalyticsClient();

				string start = startDate.ToString("yyyy-MM-dd");
				string end = endDate.ToString("yyyy-MM-dd");
				string url = $"api/advice?userId={userId}&startDate={start}&endDate={end}";

				var response = await client.GetFromJsonAsync<AnalyticsResponse<List<Advice>>>(url);
				return response ?? CreateErrorResponse<List<Advice>>("Пустой ответ от сервера");
			}
			catch (Exception ex)
			{
				return CreateErrorResponse<List<Advice>>($"Ошибка получения рекомендаций: {ex.Message}");
			}
		}

		private AnalyticsResponse<T> CreateErrorResponse<T>(string errorMessage) where T : new()
		{
			return new AnalyticsResponse<T>
			{
				IsSuccess = false,
				Error = errorMessage,
				Result = new T()
			};
		}
	}

}
