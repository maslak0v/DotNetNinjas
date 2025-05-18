using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Categories;
using Wallet.Domain.Entities;
using Wallet.Domain.Interfaces;
namespace Wallet.API.Controllers.Categories;

public class CreateCategory : CategoryBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CreateCategory(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CategoryRequest categoryRequest, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(categoryRequest);
        await _categoryService.AddAsync(category, cancellationToken);
        return Ok();
    }
}