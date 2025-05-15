using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class UserRegisterUseCase(IUserRepository repository) : IUserRegisterUseCase
    {
        public OperationResult Result { get; private set; } = null!;
        public IUserRegisterRequest Request { private get; set; } = null!;
        public async Task Execute()
        {
            try
            {
                if (!string.Equals(Request.Password, Request.ConfirmedPassword))
                {
                    Result = OperationResultCreator.Failure(
                        Enum_StatusCode.BAD_REQUEST, "Passwords are not equal.");
                    return;
                }
                
                var result = await repository.CreateUser(Request);
                if (!result.IsSuccess)
                {
                    Result = OperationResultCreator.Failure(
                        Enum_StatusCode.BAD_REQUEST, $"Failed to create user: {result.Message}");
                    return;
                }
                Result = result;
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException(ex);
            }
        }
    }
}
