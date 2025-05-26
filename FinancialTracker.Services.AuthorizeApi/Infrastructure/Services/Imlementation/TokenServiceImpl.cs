using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Models;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Imlementation
{
    public class TokenServiceImpl(
        ILogger<TokenServiceImpl> logger,
        IOptions<JwtSettings> jwtOptions,
        UserManager<AuthUser> userManager) : ITokenService<AuthUser>
    {
        public string GenerateRefreshToken() =>
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public async Task<string> GenerateTokenAsync(AuthUser user)
        {
            var claims = await GetClaims(user);
            var jwtsecurityToken = GenerateSecurityToken(claims);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtsecurityToken);
            return token;
        }

        #region private
        private JwtSecurityToken GenerateSecurityToken(IEnumerable<Claim> claims)
        {
            var jwtSettings = jwtOptions.Value;
            var secretkey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (jwtSettings is null || string.IsNullOrEmpty(secretkey))
            {
                string error = "Jwt settings is not configured";
                logger.LogWarning(error);
                throw new InvalidOperationException(error);
            }

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretkey);
            var symmetricSecurityKey = new SymmetricSecurityKey(secretKeyBytes);
            var signingCredentials = new SigningCredentials(
                    symmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.Expires),
                signingCredentials: signingCredentials);
        }

        private async Task<List<Claim>> GetClaims(AuthUser user)
        {
            List<Claim> claims = [
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Name, user.UserName!),
                new (ClaimTypes.Email, user.Email!)
            ];
            var roles = (await userManager
                .GetRolesAsync(user))
                .Select(role => new Claim(ClaimTypes.Role, role));
            claims.AddRange(roles);

            return claims;
        }
        #endregion
    }
}
