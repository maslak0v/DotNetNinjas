using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface IAuthUseCasesFacade
    {
        Task<OperationResult> UserRegisterAsync(IUserRegisterRequest request);
        Task<OperationResult<IAuthResponse>> UserLoginAsync(ITokenService tokenService, IUserLoginRequest request);
        Task<OperationResult> UserLogoutAsync(IUserLogoutRequest request);
        Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersAsync();
        Task<OperationResult<IAuthResponse>> GetUserByIdAsync(Guid id);
        Task<OperationResult<ICurrentUserLoginResponse>> GetCurrentUserAsync();
        Task<OperationResult<IAuthResponse>> UserUpdateAsync(IUserUpdateRequest reqest);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}
