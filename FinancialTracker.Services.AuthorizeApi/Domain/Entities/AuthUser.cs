using Microsoft.AspNetCore.Identity;

namespace FinancialTracker.Services.AuthorizeApi.Domain.Entities
{
    public class AuthUser: IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
