using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Tests.Helpers;
using Moq;

namespace FinancialTracker.Services.AuthorizeApi.Tests
{
    public class UserRegisterTest
    {
        [Fact]
        public async Task RegisterUserGoodResultTest()
        {
            //Arrange
            List<string> roles = [Enum_BaseRoles.USER.ToString()];

            var mockResultRegister = OperationResultCreator.Success(
                UserCreator.CreateWithUserRole(), Enum_StatusCode.CREATED, "created");
            var request = RequestCreator.CreateUserRegisterGoodRequest();
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.RegisterUserAsync(request, roles, CancellationToken.None))
                .ReturnsAsync(mockResultRegister);

            IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create(mockRepository.Object);
            IAuthUseCasesFacade useCasesFacade = AuthUseCaseFacadeCreator.Create(fabric);

            //Act
            var result = await useCasesFacade.UserRegisterAsync(request, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(Enum_StatusCode.CREATED, result.StatusCode);
        }

        [Fact]
        public async Task RegisterUserBadResultTest()
        {
            //Arrenge
            List<string> roles = [Enum_BaseRoles.USER.ToString()];
            var mockResult = OperationResultCreator.Failure<User>
                (Enum_StatusCode.BAD_REQUEST, "error message");
            var request = RequestCreator.CreateUserRegisterBadRequest();
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.RegisterUserAsync(request, roles, CancellationToken.None))
                .ReturnsAsync(mockResult);

            IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create(mockRepository.Object);
            IAuthUseCasesFacade useCasesFacade = AuthUseCaseFacadeCreator.Create(fabric);

            //Act
            var result = await useCasesFacade.UserRegisterAsync(request, CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }
    }
}
