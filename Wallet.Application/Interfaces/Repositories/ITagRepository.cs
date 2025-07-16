using Wallet.Domain.Entities;

namespace Wallet.Application.Interfaces.Repositories;

public interface ITagRepository
{
    Task CreateAsync(Tag tag, CancellationToken cancellationToken);
    Task<Tag?> GetUserTagByNameAsync(string name, Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<Tag>> SearchUserTagsByPrefixAsync(Guid userId, string prefix, int limit, int page, CancellationToken cancellationToken);
}