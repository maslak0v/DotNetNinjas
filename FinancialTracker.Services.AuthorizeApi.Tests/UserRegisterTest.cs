using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
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
            var mockResult = OperationResultCreator.Success(Enum_StatusCode.CREATED);
            var request = RequestCreator.CreateUserRegisterGoodRequest();
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.CreateUserAsync(request))
                .ReturnsAsync(mockResult);

            IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create(mockRepository.Object);
            IAuthUseCasesFacade useCasesFacade = AuthUseCaseFacadeCreator.Create(fabric);

            //Act
            var result = await useCasesFacade.UserRegisterAsync(request);

            //Assert
            mockRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<IUserRegisterRequest>()), Times.Once);
            Assert.True(result.IsSuccess);
            Assert.Equal(Enum_StatusCode.CREATED, result.StatusCode);
        }

        [Fact]
        public async Task RegisterUserBadResultTest()
        {
            //Arrenge
            var mockResult = OperationResultCreator.Failure
                (Enum_StatusCode.BAD_REQUEST, "error message");
            var request = RequestCreator.CreateUserRegisterBadRequest();
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.CreateUserAsync(request))
                .ReturnsAsync(mockResult);

            IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create(mockRepository.Object);
            IAuthUseCasesFacade useCasesFacade = AuthUseCaseFacadeCreator.Create(fabric);

            //Act
            var result = await useCasesFacade.UserRegisterAsync(request);

            //Assert
            mockRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<IUserRegisterRequest>()), Times.Once);
            Assert.False(result.IsSuccess);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }
    }
}
