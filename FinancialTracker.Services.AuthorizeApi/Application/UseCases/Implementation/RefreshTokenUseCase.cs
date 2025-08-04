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
        IAuthTokenService tokenService,
        IRefreshRequest request) : IRefreshUseCase
    {
        public OperationResult<ITokenResponse> Result { get; private set; } = null!;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                
                RefreshToken? refreshToken = await tokenService.FindRefreshTokenByJtiAsync(request.Jti, cancellationToken);
                
                //если токен уже истек,
                //то требуется пройти процедуру авторизации по новой,
                //чтобы получить валидный токен
                var validationResult = await ValidateRefreshTokenAsync(refreshToken, cancellationToken);
                if (!validationResult.IsSuccess)
                {
                    Result = OperationResultCreator.Failure<ITokenResponse>(
                        validationResult.StatusCode, validationResult.Message!);
                    return;
                }

                //отзываем токены(пока поддержка только одного устройства)
                await tokenService.RevokeAllForUserAsync(refreshToken!.UserId, cancellationToken);

                //ищем пользователя, т.к. для генерации новых токенов нужны его данные
                User? user = await userRepository.FindByIdAsync(refreshToken.UserId, cancellationToken);
                if (user is null)
                {
                    Result = OperationResultCreator.Failure<ITokenResponse>(Enum_StatusCode.INVALID_TOKEN, "User not found by token");
                    return;
                }

                //все ок, создаем новые токены
                var refreshTokenNew = await tokenService.GenerateRefreshTokenAsync(user.Id, cancellationToken);
                var accesToken = tokenService.GenerateAccessToken(user, refreshTokenNew.Jti.ToString());
                ITokenResponse response = new TokenResponse(accesToken, refreshTokenNew.Token);
                Result = OperationResultCreator.Success(response, Enum_StatusCode.OK);
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException<ITokenResponse>(ex);
            }
        }

        private async Task<OperationResult> ValidateRefreshTokenAsync(RefreshToken? refreshToken, CancellationToken cancellationToken)
        {
            string error = string.Empty;
            //токен в базе соотвствует токену в запросе?
            if (refreshToken is null)
                error = "Token not found";
            else if (!string.Equals(refreshToken!.Token, request.RefreshToken))
                error = "Tokens not equal";

            //не истек?
            else if (!refreshToken.IsValid())
            {
                //истек - сразу отзываем
                await tokenService.RevokeAsync(refreshToken, cancellationToken);
                error = "Token is not valid";
            }
            return string.IsNullOrEmpty(error)
                ? OperationResultCreator.Success(Enum_StatusCode.OK)
                : OperationResultCreator.Failure(Enum_StatusCode.INVALID_TOKEN, error);
        }
    }
}
