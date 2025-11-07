
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Tests.Helpers;
using Moq;

namespace FinancialTracker.Services.AuthorizeApi.Tests
{
    public class RevokeAllRefreshTokensTest
    {
        [Fact]
        public async Task RevokeAll()
        {
            //Arrange
            var mockTokenRepository = new Mock<ITokenRepository>();
            mockTokenRepository
                .Setup(repo => repo.RevokeAllAsync(CancellationToken.None))
                .Returns(Task.CompletedTask);

            var tokenService = AuthTokenServiceCreator.Create(JwtOptionsMocker.GetMock(), mockTokenRepository.Object);
            var fabric = AuthUseCaseFabricCreator.Create();
            var facade = AuthUseCaseFacadeCreator.Create(fabric);

            //Act
            var result = await facade.RevokeAllRefreshTokensAsync(tokenService, CancellationToken.None);

            //Assert
            mockTokenRepository.Verify(repo => repo.RevokeAllAsync(CancellationToken.None), Times.Once);
            mockTokenRepository.VerifyNoOtherCalls();
            Assert.True(result.IsSuccess);
        }
    }
}
