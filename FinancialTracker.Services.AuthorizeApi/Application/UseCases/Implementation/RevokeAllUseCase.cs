using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class RevokeAllUseCase(IAuthTokenService tokenService) : IRevokeAllUseCase
    {
        public OperationResult Result { get; private set; } = null!;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await tokenService.RevokeAllAsync(cancellationToken);
                Result = OperationResultCreator.Success(Domain.ValueObjects.Enum_StatusCode.NO_CONTENT);
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException(ex);
            }
        }
    }
}
