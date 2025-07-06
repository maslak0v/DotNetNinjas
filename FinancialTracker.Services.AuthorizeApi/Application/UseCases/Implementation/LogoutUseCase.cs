using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class LogoutUseCase(
        string userId,
        IAuthTokenService tokenService) : ILogoutUseCase
    {
        public OperationResult Result { get; private set; } = null!;

        public async Task ExecuteAsync()
        {
            try
            {
                await tokenService.RevokeAllForUserAsync(userId);
                Result = OperationResultCreator.Success(Enum_StatusCode.NO_CONTENT);
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException(ex);
            }
        }
    }
}
