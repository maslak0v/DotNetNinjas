

using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using Moq;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    internal static class JwtOptionsMocker
    {
        internal static IOptions<JwtSettings> GetMock()
        {
            var options = new Mock<IOptions<JwtSettings>>();
            options.Setup(o => o.Value).Returns(JwtSettingsCreator.Create());
            return options.Object;
        }
    }
}
