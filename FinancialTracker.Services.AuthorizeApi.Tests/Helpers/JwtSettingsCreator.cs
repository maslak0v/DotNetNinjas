using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    internal static class JwtSettingsCreator
    {
        public static JwtSettings Create() => new JwtSettings()
        {
            Expires= 15,
            RefreshExpires= 60,
            ValidAudience = "",
            ValidIssuer = "AuthWebApi"
        };
    }
}
