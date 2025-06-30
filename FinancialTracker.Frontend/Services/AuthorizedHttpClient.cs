using System.Net.Http.Headers;

namespace FinancialTracker.Frontend.Services;

public class AuthorizedHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public AuthorizedHttpClient(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        await AddTokenToHeader();
        return await _httpClient.GetAsync(requestUri);
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        await AddTokenToHeader();
        return await _httpClient.PostAsync(requestUri, content);
    }

    private async Task AddTokenToHeader()
    {
        var token = await _authService.GetAccessToken();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}