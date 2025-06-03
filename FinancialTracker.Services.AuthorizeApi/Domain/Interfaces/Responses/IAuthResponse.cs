namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses
{
    public interface IAuthResponse
    {
        string AccessToken { get; }
        string RefreshToken { get; }
    }
}
