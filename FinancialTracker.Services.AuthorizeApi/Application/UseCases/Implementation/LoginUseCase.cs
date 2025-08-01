using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class LoginUseCase(
        IUserRepository repository,
        IAuthTokenService tokenService,
        IUserLoginRequest request) : ILoginUseCase
    {
        public OperationResult<ITokenResponse> Result { get; private set; } = null!;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                User? user = await repository.TryGetCurrentLoginUserAsync(request.Email, request.Password, cancellationToken);
                if (user is null)
                {
                    Result = OperationResultCreator.Failure<ITokenResponse>(
                        Enum_StatusCode.UNAUTHORIZED, "Invalid data");
                    return;
                }
                //отзыв существующих токенов при каждом входе токены отзываются
                await tokenService.RevokeAllForUserAsync(user.Id, cancellationToken);

                //генерация новых токенов
                var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);
                var accessToken = tokenService.GenerateAccessToken(user, refreshToken.Jti.ToString());
                ITokenResponse response = new TokenResponse(accessToken, refreshToken.Token);
                Result = OperationResultCreator.Success(response, Enum_StatusCode.OK);
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException<ITokenResponse>(ex);
            }
        }
    }
}
