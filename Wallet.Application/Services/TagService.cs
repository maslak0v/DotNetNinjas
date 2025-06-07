using Wallet.Application.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Data.Interfaces;

namespace Wallet.Application.Services;

public class TagService : ITagService
{
    private ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }
    
    public async Task AddAsync(Tag tag, CancellationToken cancellationToken) 
        => await _tagRepository.CreateAsync(tag, cancellationToken); 
    
    public async Task<Tag?> GetTagByIdAsync(Guid tagId, CancellationToken cancellationToken) 
        => await _tagRepository.GetByIdAsync(tagId, cancellationToken);

    public Task<IEnumerable<Tag>> GetAllUserTagsAsync(Guid userId, CancellationToken cancellationToken) 
        => _tagRepository.GetAllUserTagsAsync(userId, cancellationToken);

    public async Task<IEnumerable<Tag>> SearchTagsByPrefixAsync(Guid userId, string prefix, int limit, CancellationToken cancellationToken) 
        => await _tagRepository.SearchTagsByPrefixAsync(userId, prefix, limit, cancellationToken);
}