using System.IdentityModel.Tokens.Jwt;

namespace FinancialTracker.Frontend.Services
{
	public class JwtService
	{
		public JwtSecurityToken ParseToken(string token)
		{
			if (string.IsNullOrEmpty(token))
				return null;

			try
			{
				var handler = new JwtSecurityTokenHandler();
				return handler.ReadJwtToken(token);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error parsing JWT: {ex.Message}");
				return null;
			}
		}

		public string GetJti(string token)
		{
			var jwtToken = ParseToken(token);
			return jwtToken?.Id; // jti хранится в свойстве Id
		}

		public string GetClaim(string token, string claimType)
		{
			var jwtToken = ParseToken(token);
			return jwtToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
		}

		public DateTime GetExpiration(string token)
		{
			var jwtToken = ParseToken(token);
			return jwtToken?.ValidTo ?? DateTime.MinValue;
		}

		public bool IsTokenExpired(string token)
		{
			var expiration = GetExpiration(token);
			return expiration < DateTime.UtcNow;
		}

		public Dictionary<string, string> GetAllClaims(string token)
		{
			var claims = new Dictionary<string, string>();
			var jwtToken = ParseToken(token);

			if (jwtToken != null)
			{
				foreach (var claim in jwtToken.Claims)
				{
					claims[claim.Type] = claim.Value;
				}
			}

			return claims;
		}

		public void PrintTokenInfo(string token)
		{
			var jwtToken = ParseToken(token);
			if (jwtToken == null)
			{
				Console.WriteLine("Invalid token");
				return;
			}

			Console.WriteLine($"JTI: {jwtToken.Id}");
			Console.WriteLine($"Issuer: {jwtToken.Issuer}");
			Console.WriteLine($"Audience: {string.Join(", ", jwtToken.Audiences)}");
			Console.WriteLine($"Issued At: {jwtToken.IssuedAt}");
			Console.WriteLine($"Expires: {jwtToken.ValidTo}");
			Console.WriteLine($"Subject: {jwtToken.Subject}");

			Console.WriteLine("Claims:");
			foreach (var claim in jwtToken.Claims)
			{
				Console.WriteLine($"  {claim.Type}: {claim.Value}");
			}
		}
	}
}
