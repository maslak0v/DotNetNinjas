using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Tags;

[ApiController]
[Route("api/user/tags")]
[ApiExplorerSettings(GroupName = "Управление тегами")]
public class TagBase : ControllerBase 
{
    protected readonly ITagService _tagService;
    protected readonly IMapper _mapper;

    public TagBase(ITagService tagService, IMapper mapper)
    {
        _tagService = tagService;
        _mapper = mapper;
    }
}