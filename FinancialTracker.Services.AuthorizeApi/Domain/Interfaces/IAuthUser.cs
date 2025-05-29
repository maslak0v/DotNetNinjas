namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces
{
    public interface IAuthUser
    {
        string? Email { get; set; }
        string? UserName { get; set; }
        string Id { get; set; }
    }
}
