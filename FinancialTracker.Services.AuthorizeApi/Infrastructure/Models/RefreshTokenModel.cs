using FinancialTracker.Services.AuthorizeApi.Domain.Entities;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Models
{
    public class RefreshTokenModel
    {
        public Guid Jti { get; set; } //key
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public AuthUser User { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        RefreshTokenModel() { }
        public RefreshTokenModel(
            string token, string userid, Guid jti, DateTime expiresAt, bool isRevoked)
        {
            Token = token;
            UserId = userid;
            ExpiresAt = expiresAt;
            Jti = jti;
            IsRevoked = isRevoked;
        }
    }
}
