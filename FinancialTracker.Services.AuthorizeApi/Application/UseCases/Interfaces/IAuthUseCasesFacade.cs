using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface IAuthUseCasesFacade
    {
        Task<OperationResult<User>> UserRegisterAsync(IUserRegisterRequest request, CancellationToken cancellationToken);
        Task<OperationResult<ITokenResponse>> UserLoginAsync(IAuthTokenService tokenService, IUserLoginRequest request, CancellationToken cancellationToken);
        Task<OperationResult> UserLogoutAsync(string userid, IAuthTokenService tokenService, CancellationToken cancellationToken);
        Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<OperationResult<ITokenResponse>> RefreshAsync(IAuthTokenService tokenService, IRefreshRequest request, CancellationToken cancellationToken);
        Task<OperationResult<ICurrentUserLoginResponse>> GetCurrentUserAsync(CancellationToken cancellationToken);
        Task<OperationResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
