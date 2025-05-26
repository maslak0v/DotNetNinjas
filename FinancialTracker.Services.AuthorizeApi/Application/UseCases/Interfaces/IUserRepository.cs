using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult> CreateUserAsync(IUserRegisterRequest userRegisterRequest);
        Task<bool> ExistUsernameAsync(string username);
        Task<bool> ExistEmailAsync(string email);

        Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersQueryAsync();
    }
}
