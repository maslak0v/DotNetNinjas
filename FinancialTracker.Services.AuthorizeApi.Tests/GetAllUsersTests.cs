
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
            List<IUserResponseInfo> users = [];
            var mockResult = OperationResultCreator.Success(users, Domain.ValueObjects.Enum_StatusCode.OK);
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.GetAllUsersQueryAsync())
                .ReturnsAsync(mockResult);

            IUseCaseFabric fabric = UseCaseFabricCreator.Create(mockRepository.Object);
            IUseCasesFacade facade = UseCaseFacadeCreator.Create(fabric);
            var result = await facade.GetAllUsersAsync();
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
                var mockResult = OperationResultCreator.FromException<List<IUserResponseInfo>>(ex);
                var mockRepository = new Mock<IUserRepository>();
                mockRepository.Setup(repo => repo.GetAllUsersQueryAsync())
                    .ReturnsAsync(mockResult);

                IUseCaseFabric fabric = UseCaseFabricCreator.Create(mockRepository.Object);
                IUseCasesFacade facade = UseCaseFacadeCreator.Create(fabric);
                var result = await facade.GetAllUsersAsync();
                mockRepository.Verify(repo => repo.GetAllUsersQueryAsync(), Times.Once);
                Assert.False(result.IsSuccess);
                Assert.Equal(result.status, mockResult.status);
            }
        }
    }
}
