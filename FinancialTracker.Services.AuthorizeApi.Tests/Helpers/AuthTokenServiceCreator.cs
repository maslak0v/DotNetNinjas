using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Imlementation;
using Microsoft.Extensions.Options;

namespace FinancialTracker.Services.AuthorizeApi.Tests.Helpers
{
    public class AuthTokenServiceCreator
    {
        public static IAuthTokenService Create(
            IOptions<JwtSettings> options,
            ITokenRepository tokenRepository) => 
            new TokenServiceImpl(options, tokenRepository);
    }
}
