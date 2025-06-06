using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Tags;
using Wallet.Application.Interfaces;
using Wallet.Domain.Entities;

namespace Wallet.API.Controllers.Tags;

public class CreateTag : TagBase
{
    private readonly ITagService _tagService;
    private readonly IMapper _mapper;

    public CreateTag(IMapper mapper, ITagService tagService)
    {
        _mapper = mapper;
        _tagService = tagService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] TagRequest tagRequest, CancellationToken cancellationToken)
    {
        var tag = _mapper.Map<Tag>(tagRequest);
        await _tagService.AddAsync(tag, cancellationToken);
        return Ok(new { tagId = tag.TagId });
    }
}