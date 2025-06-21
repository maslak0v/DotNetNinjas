using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface IAuthUseCasesFacade
    {
        Task<OperationResult> UserRegisterAsync(IUserRegisterRequest request);
        Task<OperationResult<ITokenResponse>> UserLoginAsync(IAuthTokenService tokenService, IUserLoginRequest request);
        Task<OperationResult> UserLogoutAsync(IUserLogoutRequest request);
        Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersAsync();
        Task<OperationResult<ITokenResponse>> RefreshAsync(IAuthTokenService tokenService, IRefreshRequest request);
        Task<OperationResult<ICurrentUserLoginResponse>> GetCurrentUserAsync();
        Task<OperationResult> DeleteAsync(Guid id);
    }
}
