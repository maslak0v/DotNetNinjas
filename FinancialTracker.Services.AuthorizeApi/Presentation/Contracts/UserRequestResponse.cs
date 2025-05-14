using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;

namespace FinancialTracker.Services.AuthorizeApi.Presentation.Contracts
{
    //Register
    public record UserRegisterRequest(
        string Email,
        string Password,
        string ConfirmedPassword,
        string FullName) : IUserRegisterRequest;


    //Login
    public record UserLoginRequest(string Email, string Password);
    public record CurrentUserLoginResponse(Guid Id, string AccessToken, string RefreshToken);
    public record UserResponse(Guid Id, string AccessToken, string RefreshToken);
    //Logout
    public record UserLogoutRequest(Guid Id);

    //Update
    public record UserUpdateRequest(string Email, string Password, string FullName);

    //Delete
    public record UserRemoveRequest(Guid Id);
}
