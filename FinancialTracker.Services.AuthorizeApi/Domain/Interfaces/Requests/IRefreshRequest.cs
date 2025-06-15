namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests
{
    public interface IRefreshRequest
    {
        string RefreshToken { get; }
        Guid Jti {  get; }
    }
}
