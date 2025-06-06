using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Data.Interfaces;

public interface ITagRepository
{
    Task CreateAsync(Tag tag, CancellationToken cancellationToken);
    Task<Tag?> GetByIdAsync(Guid tagId, CancellationToken cancellationToken);
    Task<Tag?> GetTagIdByNameAsync(string title, Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<Tag>> GetAllUserTagsAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<Tag>> SearchTagsByPrefixAsync(Guid userId, string prefix, int limit, CancellationToken cancellationToken);
}