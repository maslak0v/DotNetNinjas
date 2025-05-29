namespace FinancialTracker.Services.AuthorizeApi.Domain.Interfaces
{
    public interface IUser
    {
        public string Id { get; }
        public string? Email { get; }
        public string? UserName { get; }
        public DateTime CreateAt { get; }
        public DateTime? UpdateAt { get; }
    }
}
