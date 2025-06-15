using Microsoft.AspNetCore.Identity;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Models
{
    public class AuthRole: IdentityRole
    {
        public ICollection<AuthUser> Users { get; set; } = [];

        public AuthRole() { }
        public AuthRole(string role) : base(role) { }
    }
}
