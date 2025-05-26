using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts
{
    public record UserResponseInfo(string Id, string Name, string Email) 
        : IUserResponseInfo;

    //Register
    public record UserRegisterRequest(
        string Email,
        string Password,
        string ConfirmedPassword,
        string FullName) : IUserRegisterRequest;

    //Login
    public record UserLoginRequest(string Email, string Password);
    public record CurrentUserLoginResponse(string Id, string AccessToken, string RefreshToken);
    
    //Logout
    public record UserLogoutRequest(string Id);

    //Update
    public record UserUpdateRequest(string Email, string Password, string FullName);

    //Delete
    public record UserRemoveRequest(Guid Id);
}
