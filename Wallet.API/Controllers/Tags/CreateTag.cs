using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Tags;
using Wallet.Application.Dto.Tags;
using Wallet.Application.Interfaces;


namespace Wallet.API.Controllers.Tags;

public class CreateTag : TagBase
{
    public CreateTag(ITagService tagService, IMapper mapper) : base(tagService, mapper)
    {
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] TagRequest tagRequest, CancellationToken cancellationToken)
    {
        var tag = _mapper.Map<TagDto>(tagRequest);
        await _tagService.AddAsync(tag, cancellationToken);
        return NoContent();
    }
}