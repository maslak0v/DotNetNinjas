using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface IUseCasesFacade
    {
        Task<OperationResult> UserRegisterAsync(IUserRegisterRequest request);
        Task<OperationResult<IUserResponse>> UserLoginAsync(IUserLoginRequest request);
        Task<OperationResult> UserLogoutAsync(IUserLogoutRequest request);
        Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersAsync();
        Task<OperationResult<IUserResponse>> GetUserByIdAsync(Guid id);
        Task<OperationResult<ICurrentUserLoginResponse>> GetCurrentUserAsync();
        Task<OperationResult<IUserResponse>> UserUpdateAsync(IUserUpdateRequest reqest);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}
