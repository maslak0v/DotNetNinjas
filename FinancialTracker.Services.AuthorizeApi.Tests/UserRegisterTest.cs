using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinancialTracker.Services.AuthorizeApi.Tests
{
    public class UserRegisterTest
    {
        [Fact]
        public async Task RegisterUserGoodResultTest()
        {
            var mockResult = OperationResultCreator.Success(Enum_StatusCode.CREATED);
            var request = RequestCreator.CreateUserRegisterGoodRequest();
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.CreateUserAsync(request))
                .Returns(Task.FromResult(mockResult));

            IUseCaseFabric fabric = UseCaseFabricCreator.Create(mockRepository.Object);
            IUseCasesFacade useCasesFacade = UseCaseFacadeCreator.Create(fabric);
            var result = await useCasesFacade.UserRegisterAsync(request);
            mockRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<IUserRegisterRequest>()), Times.Once);

            Assert.True(result.IsSuccess);
            Assert.Equal(Enum_StatusCode.CREATED, result.StatusCode);
        }

        [Fact]
        public async Task RegisterUserBadResultTest()
        {
            var mockResult = OperationResultCreator.Failure
                (Enum_StatusCode.BAD_REQUEST, "error message");
            var request = RequestCreator.CreateUserRegisterBadRequest();
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.CreateUserAsync(request))
                .Returns(Task.FromResult(mockResult));
            

            IUseCaseFabric fabric = UseCaseFabricCreator.Create(mockRepository.Object);
            IUseCasesFacade useCasesFacade = UseCaseFacadeCreator.Create(fabric);
            var result = await useCasesFacade.UserRegisterAsync(request);
            mockRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<IUserRegisterRequest>()), Times.Once);
            Assert.False(result.IsSuccess);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }
    }
}
