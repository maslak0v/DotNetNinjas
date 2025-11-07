namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests
{
    public interface IUserRegisterRequest
    {
        string Email { get; }
        string Password { get; }
        string ConfirmedPassword { get; }
        string FullName { get; }
    }
}
