
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class GetAllUsersUseCase(IUserRepository userRepository) : IGetAllUsersUseCase
    {
        public OperationResult<List<IUserResponseInfo>> Result { get; private set; } = null!;

        public async Task Execute()
        {
            try
            {
                Result = await userRepository.GetAllUsersQueryAsync();
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException<List<IUserResponseInfo>>(ex);
            }
        }
    }
}
