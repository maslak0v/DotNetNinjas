
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Tests.Helpers;
using Moq;

namespace FinancialTracker.Services.AuthorizeApi.Tests
{
    public class UserLogoutTest
    {
        [Fact]
        public async Task UserLogout()
        {
            //Arrange
            var user = UserCreator.CreateWithUserRole();
            var mockUserRepository = new Mock<IUserRepository>();

            var mockTokenRepository = new Mock<ITokenRepository>();
            mockTokenRepository
                .Setup(repo => repo.RevokeAllForUserAsync(user.Id))
                .Returns(Task.CompletedTask);

            var fabric = AuthUseCaseFabricCreator.Create(mockUserRepository.Object);
            var facade = AuthUseCaseFacadeCreator.Create(fabric);
            var tokenService = AuthTokenServiceCreator.Create(
                JwtOptionsMocker.GetMock(),
                mockTokenRepository.Object);

            //Act
            await facade.UserLogoutAsync(user.Id, tokenService);

            //Assert
            mockTokenRepository.Verify(repo => repo.RevokeAllForUserAsync(user.Id), Times.Once);
            mockTokenRepository.VerifyNoOtherCalls();
            mockUserRepository.VerifyNoOtherCalls();    
        }
    }
}
