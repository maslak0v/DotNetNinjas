
namespace FinancialTracker.Services.AuthorizeApi.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Jti { get; set; }
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }

        private RefreshToken (string token, string userid, DateTime expiresAt)
        {
            Token = token;
            UserId = userid;
            ExpiresAt = expiresAt;
            Jti = Guid.CreateVersion7(TimeProvider.System.GetUtcNow());
        }

        private RefreshToken(Guid jti, string token, string userId, DateTime expiresAt, bool isRevoked)
        {
            Jti = jti;
            Token = token;
            UserId = userId;
            ExpiresAt = expiresAt;
            IsRevoked = isRevoked;
        }
        public static RefreshToken CreateNew(string token, string userid, DateTime expiresAt)
            => new RefreshToken(token, userid, expiresAt);
        public static RefreshToken CopyData(Guid jti, string token, string userId, DateTime expiresAt, bool isRevoked)
            => new RefreshToken(jti, token, userId, expiresAt, isRevoked);
        public void  Revoke() => IsRevoked = true;
        public bool IsValid() => !IsRevoked && ExpiresAt > DateTime.UtcNow;
    }
}
