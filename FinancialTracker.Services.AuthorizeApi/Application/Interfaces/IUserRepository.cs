using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.Interfaces
{
    public interface IUserRepository
    {
        //Task<OperationResult> CreateUserAsync(IUserRegisterRequest userRegisterRequest);
        Task<User?> TryGetCurrentLoginUserAsync(string email, string password);
        Task<User?> FindByIdAsync(string userId);
        Task<bool> ExistUserNameAsync(string username);
        Task<bool> ExistEmailAsync(string email);

        Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersQueryAsync();
        Task<OperationResult> AddRolesToUserAsync(User user, ICollection<string> roles);
        Task<IList<string>> GetRolesForUserAsync(User user);
        Task<OperationResult<User>> RegisterUserAsync(
            IUserRegisterRequest request, ICollection<string> roles);
    }
}
