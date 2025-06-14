using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Tags;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Tags;

public class GetAllTagsByUserId : TagBase
{
    public GetAllTagsByUserId(ITagService tagService, IMapper mapper) : base(tagService, mapper)
    {
    }

    [HttpGet("{id:guid}/all")]
    public async Task<IActionResult> GetAll([FromRoute] Guid id,  CancellationToken cancellationToken)
    {
        var tags = await _tagService.GetAllUserTagsAsync(id,cancellationToken);
        var response = _mapper.Map<List<TagResponse>>(tags);
        return Ok(response);
    }
}