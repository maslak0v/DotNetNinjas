using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Categories;
using Wallet.Domain.Interfaces;

namespace Wallet.API.Controllers.Categories;

public class GetAllCategories : CategoryBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    
    public GetAllCategories(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        var response = _mapper.Map<List<CategoryResponse>>(categories);
        return Ok(response);
    }
}