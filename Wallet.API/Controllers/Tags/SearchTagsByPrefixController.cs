using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Tags;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Tags;

public class SearchTagsByPrefixController : TagBase
{
    public SearchTagsByPrefixController(ITagService tagService, IMapper mapper) : base(tagService, mapper)
    {
    }

    [HttpGet("{userId:guid}/search")]
    public async Task<IActionResult> Search([FromRoute] Guid userId, [FromQuery][MinLength(1)] string prefix, [FromQuery][Range(1, 100, ErrorMessage = "Недопустимый лимит выборки")] int limit,
        CancellationToken cancellationToken)
    {
        var tags = await _tagService.SearchTagsByPrefixAsync(userId, prefix, limit, cancellationToken);
        var response = _mapper.Map<List<TagResponse>>(tags);
        
        return Ok(response);
    }
}