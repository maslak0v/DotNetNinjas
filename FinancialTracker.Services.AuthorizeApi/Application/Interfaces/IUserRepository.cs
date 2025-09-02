using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.Interfaces
{
    public interface IUserRepository
    {
        //Task<OperationResult> CreateUserAsync(IUserRegisterRequest userRegisterRequest);
        Task<User?> TryGetCurrentLoginUserAsync(string email, string password, CancellationToken cancellationToken);
        Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken);
        Task<bool> ExistUserNameAsync(string username, CancellationToken cancellationToken);
        Task<bool> ExistEmailAsync(string email, CancellationToken cancellationToken);

        Task<OperationResult<List<IUserResponseInfo>>> GetAllUsersQueryAsync(CancellationToken cancellationToken);
        Task<OperationResult> AddRolesToUserAsync(User user, ICollection<string> roles, CancellationToken cancellationToken);
        Task<IList<string>> GetRolesForUserAsync(User user, CancellationToken cancellationToken);
        Task<OperationResult<User>> RegisterUserAsync(
            IUserRegisterRequest request, ICollection<string> roles, CancellationToken cancellationToken);

        Task<OperationResult> DeleteAsync(string userId, CancellationToken cancellationToken);     
    }
}
