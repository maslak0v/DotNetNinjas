using Microsoft.AspNetCore.Identity;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Models
{
    public class AuthUser: IdentityUser
    {
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public ICollection<RefreshTokenModel> RefreshTokens { get; set; } = [];
        public ICollection<AuthRole> Roles { get; set; } = [];
    }
}
