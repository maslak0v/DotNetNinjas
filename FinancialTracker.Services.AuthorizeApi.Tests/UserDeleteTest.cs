using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Features;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Tests.Helpers;
using Moq;
namespace FinancialTracker.Services.AuthorizeApi.Tests
{
    public class UserDeleteTest
    {
        [Theory]
        [InlineData(false)]//случай невалидного(отсутсвующего id)
        [InlineData(true)] //хороший случай, userId существует
        public async Task UserDelete(bool goodUserId)
        {
            //Arrange

            var mockResultDeleting = goodUserId
                ? OperationResultCreator.Success(Enum_StatusCode.NO_CONTENT, string.Empty)
                : OperationResultCreator.Failure(Enum_StatusCode.NOT_FOUND, "NotFound");
                
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(mockResultDeleting);

            IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create();
            IAuthUseCasesFacade facade = AuthUseCaseFacadeCreator.Create(fabric);

            //Act
            var result = await facade.DeleteAsync(Guid.CreateVersion7().ToString(), mockUserRepository.Object, CancellationToken.None);

            //Assert
            if (goodUserId)
                Assert.Equal(Enum_StatusCode.NO_CONTENT, result.StatusCode);
            else
            {
                Assert.False(result.IsSuccess);
                Assert.False( string.IsNullOrEmpty(result.Message));
                Assert.Equal(Enum_StatusCode.NOT_FOUND, result.StatusCode);
            }

        }
    }
}
