using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Categories;

[ApiController]
[Route("api/categories")]
[ApiExplorerSettings(GroupName = "Управление категориями")]
public class CategoryBase: ControllerBase
{
    protected readonly ICategoryService _categoryService;
    protected readonly IMapper _mapper;

    public CategoryBase(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }
}