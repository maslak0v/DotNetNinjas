namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses
{
    public interface ITokenResponse
    {
        string AccessToken { get; }
        string RefreshToken { get; }
    }
}
