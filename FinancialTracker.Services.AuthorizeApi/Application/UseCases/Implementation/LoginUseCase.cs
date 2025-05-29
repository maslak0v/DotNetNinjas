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
        ITokenService tokenService,
        IUserLoginRequest request) : ILoginUseCase
    {
        public OperationResult<IAuthResponse> Result { get; private set; } = null!;

        public async Task ExecuteAsync()
        {
            try
            {
                //try login
                User? user = await repository.TryGetCurrentLoginUserAsync(request.Email, request.Password);
                if (user is null)
                {
                    Result = OperationResultCreator.Failure<IAuthResponse>(
                        Enum_StatusCode.UNAUTHORIZED, "Invalid data");
                    return;
                }
                //Create tokens for response
                var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id);
                var roles = await repository.GetRolesForUserAsync(user);
                var accesToken = tokenService.GenerateAccessToken(user, refreshToken.Jti.ToString(), roles);
                IAuthResponse response = new AuthResponse(accesToken, refreshToken.Token);
                Result = OperationResultCreator.Success(response, Enum_StatusCode.OK);
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException<IAuthResponse>(ex);
            }
        }
    }
}
