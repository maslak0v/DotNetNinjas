using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class DeleteUseCase(
        string userId,
        IUserRepository userRepository) : IDeleteUseCase
    {
        public OperationResult Result { get; private set; } = null!;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                Result = await userRepository.DeleteAsync(userId, cancellationToken);
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException(ex);
            }
        }
    }
}
