using FinancialTracker.Services.AuthorizeApi.Application.Interfaces;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;
using FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Services.Imlementation
{
    public class TokenServiceImpl(
        IOptions<JwtSettings> jwtOptions,
        ITokenRepository tokenRepository) : ITokenService
    {
        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            string refreshtokenStr = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var expiresDays = jwtOptions.Value.RefreshExpires;
            RefreshToken token = RefreshToken.CreateNew(
                refreshtokenStr,
                userId,
                DateTime.UtcNow.AddDays(expiresDays));
            tokenRepository.Add(token);
            await tokenRepository.SaveAsync();
            return token;
        }

        public string GenerateAccessToken(User user, string jti, IList<string> roles)
        {
            var claims = GetClaims(user, jti, roles);
            var jwtsecurityToken = GenerateSecurityToken(claims);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtsecurityToken);
            return token;
        }

        #region private
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private JwtSecurityToken GenerateSecurityToken(IEnumerable<Claim> claims)
        {
            var jwtSettings = jwtOptions.Value;
            var secretkey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (jwtSettings is null || string.IsNullOrEmpty(secretkey))
                throw new InvalidOperationException("Jwt settings is not configured");

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretkey);
            var symmetricSecurityKey = new SymmetricSecurityKey(secretKeyBytes);
            var signingCredentials = new SigningCredentials(
                    symmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.Expires),
                signingCredentials: signingCredentials);
        }

        private List<Claim> GetClaims(User user, string jti, IList<string> roles)
        {
            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            ];
            var claimRoles = roles.Select(role => new Claim(ClaimTypes.Role, role));
            claims.AddRange(claimRoles);
            return claims;
        }
        #endregion
    }
}
