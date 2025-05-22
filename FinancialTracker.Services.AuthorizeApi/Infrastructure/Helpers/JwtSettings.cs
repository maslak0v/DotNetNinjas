namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Helpers
{
    public class JwtSettings
    {
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public double Expires { get; set; }
    }
}
