
using FinancialTracker.Services.AuthorizeApi.Application.Fabrics;
using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Requests;
using FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;
using FinancialTracker.Services.AuthorizeApi.Tests.Helpers;
using Microsoft.Extensions.Options;
using Moq;

namespace FinancialTracker.Services.AuthorizeApi.Tests
{
    public class UserLoginTest
    {
        [Fact]
        public async Task UserLoginAndGetTokens()
        {
            //Arrange
            var roles = new List<string> { "User", "Admin" };
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com"
            };
            var request = new Mock<IUserLoginRequest>();
            request.Setup(r => r.Email).Returns("test@example.com");
            request.Setup(r => r.Password).Returns("password123");

            var options = new Mock<IOptions<JwtSettings>>();
            options.Setup(o => o.Value).Returns(JwtSettingsCreator.Create());

                //user repository
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo
                .TryGetCurrentLoginUserAsync(request.Object.Email, request.Object.Password, CancellationToken.None))
                .ReturnsAsync(user);
            mockUserRepository.Setup(repo => repo.GetRolesForUserAsync(user, CancellationToken.None))
                .ReturnsAsync(roles);

                //token repository
            var mockTokenRepository = new Mock<ITokenRepository>();
            mockTokenRepository.Setup(repo => repo.SaveAsync(CancellationToken.None)).Returns(Task.CompletedTask);
            mockTokenRepository.Setup(repo => repo.RevokeAllForUserAsync(user.Id, CancellationToken.None)).Returns(Task.CompletedTask);

            IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create(mockUserRepository.Object);
            IAuthUseCasesFacade facade = AuthUseCaseFacadeCreator.Create(fabric);
            IAuthTokenService tokenService = AuthTokenServiceCreator.Create(options.Object, mockTokenRepository.Object);

            //Act
            var result = await facade.UserLoginAsync(tokenService, request.Object, CancellationToken.None);

            //Assert
            mockTokenRepository.Verify(repo => repo.SaveAsync(CancellationToken.None), Times.Once);
            mockTokenRepository.Verify(repo => repo.RevokeAllForUserAsync(user.Id, CancellationToken.None), Times.Once);
            Assert.False(string.IsNullOrEmpty(result.Result!.AccessToken), "Access token should not be null or empty");
            Assert.False(string.IsNullOrEmpty(result.Result.RefreshToken), "Refresh token should not be null or empty");
        }

        [Fact]
        public async Task UserUnauthorized()
        {
            //Arrange
            var roles = new List<string> { "User", "Admin" };
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@example.com"
            };
            var request = new Mock<IUserLoginRequest>();
            request.Setup(r => r.Email).Returns("test@example.com");
            request.Setup(r => r.Password).Returns("password123");

            var options = new Mock<IOptions<JwtSettings>>();
            options.Setup(o => o.Value).Returns(JwtSettingsCreator.Create());

                //user repository
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo
                .TryGetCurrentLoginUserAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((User?)null);

                //token repository
            var mockTokenRepository = new Mock<ITokenRepository>();
            mockTokenRepository.Setup(repo => repo.SaveAsync(CancellationToken.None)).Returns(Task.CompletedTask);
            mockTokenRepository.Setup(repo => repo.RevokeAllForUserAsync(user.Id, CancellationToken.None))
                .Returns(Task.CompletedTask);

            IAuthUseCaseFabric fabric = AuthUseCaseFabricCreator.Create(mockUserRepository.Object);
            IAuthUseCasesFacade facade = AuthUseCaseFacadeCreator.Create(fabric);
            IAuthTokenService tokenService = AuthTokenServiceCreator.Create(options.Object, mockTokenRepository.Object);

            //Act
            var result = await facade.UserLoginAsync(tokenService, request.Object, CancellationToken.None);

            //Assert
            Assert.Equal(Enum_StatusCode.UNAUTHORIZED, result.StatusCode);
        }
    }
}
