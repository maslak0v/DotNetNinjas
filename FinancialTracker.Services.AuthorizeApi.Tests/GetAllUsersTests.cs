
using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses;
using FinancialTracker.Services.AuthorizeApi.Tests.Helpers;
using Moq;

namespace FinancialTracker.Services.AuthorizeApi.Tests
{
    public class GetAllUsersTests
    {
        [Fact]
        public async Task GetAllUsersGood()
        {
            //Arrange
            List<IUserResponseInfo> users = [];
            var mockResult = OperationResultCreator.Success(users, Domain.ValueObjects.Enum_StatusCode.OK);
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetAllUsersQueryAsync())
                .ReturnsAsync(mockResult);

            IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create(mockRepository.Object);
            IAuthUseCasesFacade facade = AuthUseCaseFacadeCreator.Create(fabric);

            //Act
            var result = await facade.GetAllUsersAsync();

            //Assert
            mockRepository.Verify(repo => repo.GetAllUsersQueryAsync(), Times.Once);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetAllUsersBad()
        {
            try { throw new Exception("Internal server error"); }
            catch (Exception ex)
            {
                //Arrange
                var mockResult = OperationResultCreator.FromException<List<IUserResponseInfo>>(ex);
                var mockRepository = new Mock<IUserRepository>();
                mockRepository.Setup(repo => repo.GetAllUsersQueryAsync())
                    .ReturnsAsync(mockResult);

                IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create(mockRepository.Object);
                IAuthUseCasesFacade facade = AuthUseCaseFacadeCreator.Create(fabric);

                //Act
                var result = await facade.GetAllUsersAsync();

                //Assert
                mockRepository.Verify(repo => repo.GetAllUsersQueryAsync(), Times.Once);
                Assert.False(result.IsSuccess);
                Assert.Equal(result.status, mockResult.status);
            }
        }
    }
}
