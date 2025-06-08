using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class UserRegisterUseCase(
        IUserRepository repository,
        IUserRegisterRequest request) : IUserRegisterUseCase
    {
        public OperationResult Result { get; private set; } = null!;
        public async Task ExecuteAsync()
        {
            try
            {
                if (!string.Equals(request.Password, request.ConfirmedPassword))
                {
                    Result = OperationResultCreator.Failure(
                        Enum_StatusCode.BAD_REQUEST, "Passwords are not equal.");
                    return;
                }

                //register
                List<string> roles = [Enum_BaseRoles.USER.ToString()];
                var result = await repository.RegisterUserAsync(request, roles);
                Result = result;
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException(ex);
            }
        }
    }
}
