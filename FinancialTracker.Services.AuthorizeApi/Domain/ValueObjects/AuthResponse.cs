using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects
{
    public record AuthResponse(string AccessToken, string RefreshToken) : IAuthResponse;
}
