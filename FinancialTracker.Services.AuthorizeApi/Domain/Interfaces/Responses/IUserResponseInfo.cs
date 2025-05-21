namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces.Responses
{
    public interface IUserResponseInfo
    {
        string Name { get; }
        string Email { get; }
        string Id { get; }
    }
}
