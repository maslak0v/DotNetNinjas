using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;

namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Implementation
{
    public class UserRegisterUseCase(
        IUserRepository repository,
        ILogger<UserRegisterUseCase> logger) : IUserRegisterUseCase
    {
        public OperationResult Result { get; private set; } = null!;
        public IUserRegisterRequest Request { private get; set; } = null!;
        public async Task Execute()
        {
            try
            {
                if (!string.Equals(Request.Password, Request.ConfirmedPassword))
                {
                    Result = ErrorHandler.HandleWarningError(logger, "Passwords are not equal.");
                    return;
                }
                var checkResult = await checkExistUserAsync(Request);
                if (!checkResult.IsSuccess)
                {
                    Result = ErrorHandler.HandleWarningError(logger, checkResult.Error!);
                    return;
                }
                logger.LogInformation("Registration of a new user...");
                var result = await repository.CreateUser(Request);
                if (!result.IsSuccess)
                {
                    Result = ErrorHandler.HandleWarningError(logger, $"Failed to create user: {result.Error}");
                    return;
                }
                string message = "User created successfully";
                logger.LogInformation(message);
                result = OperationResultCreator.Success(message);
            }
            catch (Exception ex)
            {
                Result = OperationResultCreator.FromException(ex);
            }

            //------------------ Footer --------------------------
            //helpers methods
            async Task<OperationResult> checkExistUserAsync(IUserRegisterRequest request)
            {
                //сначала проверить проверку уникальности из коробки Identity
                //if (await ExistEmailAsync(request.Email))
                //{
                //    ErrorHandler.HandleWarningError(logger, "Email already exist");
                //}

                return await repository.ExistUsernameAsync(request.FullName)
                     ? ErrorHandler.HandleWarningError(logger, "User's name already exist")
                     : OperationResultCreator.Success();
            }
        }
    }
}
