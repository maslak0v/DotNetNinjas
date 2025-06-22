using FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Interfaces;

namespace FinancialTracker.Services.AuthorizeApi.Infrastructure.Contracts.Implementations
{
    public record EventUserCtreatedDTO : IEventUserCreated
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public string UserName { get; set; } = null!;
    }
}
