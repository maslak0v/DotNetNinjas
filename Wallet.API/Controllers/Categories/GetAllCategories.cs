using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Categories;
using Wallet.Application.Interfaces.Services;

namespace Wallet.API.Controllers.Categories;

public class GetAllCategories : CategoryBase
{
    public GetAllCategories(ICategoryService categoryService, IMapper mapper) : base(categoryService, mapper)
    {
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        var response = _mapper.Map<List<CategoryResponse>>(categories);
        return Ok(response);
    }
}