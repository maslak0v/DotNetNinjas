namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public double Expires { get; set; }
    }
}
