using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects
{
    public record TokenResponse(string AccessToken, string RefreshToken) : ITokenResponse;
}
