namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests
{
    public interface IUserLoginRequest
    {
        string Email { get; }
        string Password { get; }
    }
}
