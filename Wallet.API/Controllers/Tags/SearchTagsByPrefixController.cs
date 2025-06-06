using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Tags;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Tags;

public class SearchTagsByPrefixController : TagBase
{
    private readonly ITagService _tagService;
    private readonly IMapper _mapper;

    public SearchTagsByPrefixController(ITagService tagService, IMapper mapper)
    {
        _tagService = tagService;
        _mapper = mapper;
    }

    [HttpGet("{userId:guid}/search")]
    public async Task<IActionResult> Search([FromRoute] Guid userId, [FromQuery][MinLength(1)] string prefix, [FromQuery] int limit,
        CancellationToken cancellationToken)
    {
        if (prefix.Length < 1)
        {
            return BadRequest("Нужен минимум 1 знак для поиска");
        }
        
        var tags = await _tagService.SearchTagsByPrefixAsync(userId, prefix, limit, cancellationToken);
        var response = _mapper.Map<List<TagResponse>>(tags);
        
        return Ok(response);
    }
}