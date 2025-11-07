using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Implementations
{
    public record UserResponseInfo(
        [Required] string Id,
        [Required] string Name,
        [Required] string Email) 
        : IUserResponseInfo;

    //Register
    public record UserRegisterRequest(
        [Required] string Email,
        [Required] string Password,
        [Required] string ConfirmedPassword,
        [Required] string FullName,
        [Required] string CaptchaToken) : IUserRegisterRequest;

    //Login
    public record UserLoginRequest([Required]string Email, [Required] string Password)
        : IUserLoginRequest;
    
    /* future
    //Update
    public record UserUpdateRequest(string Email, string Password, string FullName);

    //Delete
    public record UserRemoveRequest(Guid Id);*/
}
