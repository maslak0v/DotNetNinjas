

namespace FinancialTracker.Services.AuthorizeApi.Domain.Entities
{
    public class User
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public IList<string> Roles { get; set; } = [];
    }
}
