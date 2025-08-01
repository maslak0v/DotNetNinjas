using Wallet.Application.Dto.Tags;

namespace Wallet.Application.Interfaces.Services;

public interface ITagService
{
    Task AddAsync(TagDto tag, CancellationToken cancellationToken);
    // Поиск тегов юзера по названию
    Task<IEnumerable<TagDto>> SearchUserTagsByPrefixAsync(Guid userId, string? prefix, int limit, int page, CancellationToken cancellationToken);
}