

using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;
using FinancialTracker.Services.AuthorizeApi.Tests.Helpers;
using Microsoft.Extensions.Options;
using Moq;

namespace FinancialTracker.Services.AuthorizeApi.Tests
{
    public class RefreshTest
    {
        [Fact]
        public async Task RefreshGoodTest() 
        {
            //Arrange
            var user = UserCreator.CreateWithUserRole();
            var refreshToken = RefreshTokenCreator.Create(user.Id, "token");
                //mocks
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(repo => repo.FindByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(user);

            var mockTokenRepo = new Mock<ITokenRepository>();
            mockTokenRepo.Setup(repo => repo.FindByJtiAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(refreshToken);
            mockTokenRepo.Setup(repo => repo.RevokeAsync(refreshToken, CancellationToken.None))
                .Returns(Task.CompletedTask);
            mockTokenRepo.Setup(repo => repo.RevokeAllForUserAsync(user.Id, CancellationToken.None))
                .Returns(Task.CompletedTask);

            var options = new Mock<IOptions<JwtSettings>>();
            options.Setup(o => o.Value).Returns(JwtSettingsCreator.Create());

            var request = new Mock<IRefreshRequest>();
            request.Setup(r => r.Jti).Returns(refreshToken.Jti);
            request.Setup(r => r.RefreshToken).Returns(refreshToken.Token);

            var usecaseFabric = AuthUseCaseFabricCreator.Create();
            var facade = AuthUseCaseFacadeCreator.Create(usecaseFabric);
            var tokenService = AuthTokenServiceCreator.Create(options.Object, mockTokenRepo.Object);

            //Act
            var response = await facade.RefreshAsync(
                mockUserRepo.Object,
                tokenService,
                request.Object,
                CancellationToken.None);

            //Assert
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Result);
            Assert.NotEmpty(response.Result.AccessToken);
            Assert.NotEmpty(response.Result.RefreshToken);
        }

        [Fact]
        public async Task RefreshBad_NotEqualsTest()
        {
            //Arrange
            var user = UserCreator.CreateWithUserRole();
            var refreshToken = RefreshTokenCreator.Create(user.Id, "token");
            //mocks
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(repo => repo.FindByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(user);

            var mockTokenRepo = new Mock<ITokenRepository>();
            mockTokenRepo.Setup(repo => repo.FindByJtiAsync(Guid.NewGuid(), CancellationToken.None))
                .ReturnsAsync(refreshToken);
            mockTokenRepo.Setup(repo => repo.RevokeAsync(refreshToken, CancellationToken.None))
                .Returns(Task.CompletedTask);
            mockTokenRepo.Setup(repo => repo.RevokeAllForUserAsync(user.Id, CancellationToken.None))
                .Returns(Task.CompletedTask);

            var options = new Mock<IOptions<JwtSettings>>();
            options.Setup(o => o.Value).Returns(JwtSettingsCreator.Create());

            var request = new Mock<IRefreshRequest>();
            request.Setup(r => r.Jti).Returns(refreshToken.Jti);
            request.Setup(r => r.RefreshToken).Returns("token2");

            var usecaseFabric = AuthUseCaseFabricCreator.Create();
            var facade = AuthUseCaseFacadeCreator.Create(usecaseFabric);
            var tokenService = AuthTokenServiceCreator.Create(options.Object, mockTokenRepo.Object);

            //Act
            var response = await facade.RefreshAsync(mockUserRepo.Object, tokenService, request.Object, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.Equal( Enum_StatusCode.INVALID_TOKEN, response.StatusCode);
        }
    }
}
