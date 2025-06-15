using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Categories;
using Wallet.Application.Interfaces;

namespace Wallet.API.Controllers.Categories;

public class GetCategoryById : CategoryBase
{
    public GetCategoryById(ICategoryService categoryService, IMapper mapper) : base(categoryService, mapper)
    {
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetByIdAsync(id, cancellationToken);
        var response = _mapper.Map<CategoryResponse>(category);
        return Ok(response);
    }
}