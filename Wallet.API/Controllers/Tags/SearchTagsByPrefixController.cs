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
    public async Task<IActionResult> Search(
        [FromRoute] Guid userId,
        [FromQuery] string? prefix,
        CancellationToken cancellationToken,
        [FromQuery][Range(1, 100, ErrorMessage = "Недопустимый лимит выборки")] int limit = 20,
        [FromQuery][Range(1, int.MaxValue, ErrorMessage = "Недопустимый номер страницы")] int page = 1)
    {
        if (page < 1)
            page = 1;

        var tags = await _tagService.SearchTagsByPrefixAsync(userId, prefix, limit, page, cancellationToken);
        var response = _mapper.Map<List<TagResponse>>(tags);

        return Ok(response);
    }
}