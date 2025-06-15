using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class RefreshTokenUseCase(
        IUserRepository userRepository,
        IAuthTokenService service,
        IRefreshRequest request) : IRefreshUseCase
    {
        public OperationResult<ITokenResponse> Result { get; private set; } = null!;

        public async Task ExecuteAsync()
        {
            try
            {
                //если токен не проходит проверки,
                //то требуется пройти процедуру авторизации по новой,
                //чтобы получить валидный токен

                //токен в базе соотвствует токену в запросе?
                RefreshToken? refreshToken = await service.FindRefreshTokenByJtiAsync(request.Jti);
                if (refreshToken is null)
                {
                    Result = OperationResultCreator.Failure<ITokenResponse>
                        (Enum_StatusCode.INVALID_TOKEN, "Token not found");
                    return;
                }
                if (!string.Equals(refreshToken.Token, request.RefreshToken))
                {
                    Result = OperationResultCreator.Failure<ITokenResponse>
                        (Enum_StatusCode.INVALID_TOKEN, "Tokens not equal");
                    return;
                }

                //не истек?
                if(!refreshToken!.IsValid())
                {
                    await service.Revoke(refreshToken);
                    Result = OperationResultCreator.Failure<ITokenResponse>
                        (Enum_StatusCode.INVALID_TOKEN, "Token is not valid");
                    return;
                }

                //отзываем токены
                await service.RevokeAllForUserAsync(refreshToken.UserId);

                //ищем пользователя, т.к. для генерации новых токенов нужны его данные
                User? user = await userRepository.FindByIdAsync(refreshToken.UserId);
                if (user is null)
                {
                    Result = OperationResultCreator.Failure<ITokenResponse>(Enum_StatusCode.INVALID_TOKEN, "User not found by token");
                    return;
                }

                //все ок, создаем новые токены
                var refreshTokenNew = await service.GenerateRefreshTokenAsync(user.Id);
                var accesToken = service.GenerateAccessToken(user, refreshTokenNew.Jti.ToString());
                ITokenResponse response = new TokenResponse(accesToken, refreshTokenNew.Token);
                Result = OperationResultCreator.Success(response, Enum_StatusCode.OK);
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException<ITokenResponse>(ex);
            }
        }
    }
}
