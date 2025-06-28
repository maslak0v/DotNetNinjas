using System.Net.Http.Json;
using System.Net.Http.Headers;
using FinancialTracker.Frontend.Models;
using Microsoft.AspNetCore.Components;

namespace FinancialTracker.Frontend.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly BrowserStorage _browserStorage;
    
    public AuthService(HttpClient httpClient, NavigationManager navigationManager, BrowserStorage browserStorage)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _browserStorage = browserStorage;
    }
    
    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/authorize/login", request);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Login failed");
        }
        
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        await StoreTokens(authResponse);
        return authResponse;
    }
    
    public async Task Logout()
    {
        await _browserStorage.RemoveAsync("accessToken");
        await _browserStorage.RemoveAsync("refreshToken");
        _navigationManager.NavigateTo("/login");
    }
    
    public async Task Register(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/authorize/register", request);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Registration failed");
        }
    }
    
    public async Task RefreshToken()
    {
        var refreshToken = await _browserStorage.GetAsync("refreshToken");
        
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new Exception("No refresh token available");
        }
        
        var response = await _httpClient.PostAsJsonAsync("api/authorize/refresh", 
            new { RefreshToken = refreshToken });
        
        if (!response.IsSuccessStatusCode)
        {
            await Logout();
            throw new Exception("Token refresh failed");
        }
        
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        await StoreTokens(authResponse);
    }
    
    private async Task StoreTokens(AuthResponse authResponse)
    {
        await _browserStorage.SetAsync("accessToken", authResponse.AccessToken);
        await _browserStorage.SetAsync("refreshToken", authResponse.RefreshToken);
    }
    
    public async Task<string> GetAccessToken()
    {
        return await _browserStorage.GetAsync("accessToken");
    }
    
    public async Task<bool> IsAuthenticated()
    {
        var token = await GetAccessToken();
        return !string.IsNullOrEmpty(token);
    }
}