namespace FinancialTracker.Services.AuthorizeApi.Application.Contracts
{
    public interface ICurrentUserService
    {
        string? GetUserId();
    }
}