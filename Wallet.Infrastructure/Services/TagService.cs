using AutoMapper;
using Wallet.Application.Dto.Tags;
using Wallet.Application.Interfaces.Repositories;
using Wallet.Application.Interfaces.Services;
using Wallet.Domain.Entities;

namespace Wallet.Infrastructure.Services;

public class TagService : ITagService
{
    private ITagRepository _tagRepository;
    private readonly IMapper _mapper;
    public TagService(ITagRepository tagRepository, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(TagDto tagDto, CancellationToken cancellationToken)
    {   
        var tag = _mapper.Map<Tag>(tagDto);
        await _tagRepository.CreateAsync(tag, cancellationToken); 
    }
    
    public async Task<IEnumerable<TagDto>> SearchUserTagsByPrefixAsync(Guid userId, string? prefix, int limit, int page, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.SearchUserTagsByPrefixAsync(userId, prefix, limit, page, cancellationToken);
        return _mapper.Map<IEnumerable<TagDto>>(tags);
    }
}