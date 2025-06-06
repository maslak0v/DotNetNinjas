using Wallet.Domain.Entities;

namespace Wallet.Application.Interfaces;

public interface ITagService
{
    Task AddAsync(Tag tag, CancellationToken cancellationToken);
    
    Task<Tag?> GetTagByIdAsync(Guid tagId, CancellationToken cancellationToken);
    
    Task<IEnumerable<Tag>> GetAllUserTagsAsync(Guid userId, CancellationToken cancellationToken);

    // Поиск тегов юзера по названию
    Task<IEnumerable<Tag>> SearchTagsByPrefixAsync(Guid userId, string prefix, int limit, CancellationToken cancellationToken);
}