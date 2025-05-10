namespace FinancialTracker.Services.AuthorizeApi.Application.UseCases.Interfaces
{
    public interface ICurrentUserService
    {
        string? GetUserId();
    }
}
