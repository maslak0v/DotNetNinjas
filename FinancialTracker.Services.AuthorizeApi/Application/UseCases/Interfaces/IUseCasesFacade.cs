using FinancialTracker.Services.AuthorizeApi.Application.Contracts;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface IUseCasesFacade
    {
        Task UserRegisterAsync(UserRegisterRequest request);
        Task<UserResponse> UserLoginAsync(UserLoginRequest request);
        Task UserLogoutAsync(UserLogoutRequest request);
        Task<UserResponse> GetUserByIdAsync(Guid id);
        Task<CurrentUserLoginResponse> GetCurrentUserAsync();
        Task<UserResponse> UserUpdateAsync(Guid id, UserUpdateRequest reqest);
        Task DeleteAsync(Guid id);

        //Task<RevokeRefreshTokenResponse> RevokeRefreshTokenAsync(RefreshTokenRemoveRequest request);
        //Task<CurrentUserLoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
