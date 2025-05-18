using AutoMapper;
using Wallet.API.Models.Categories;
using Wallet.Domain.Entities;

namespace Wallet.API.Mappings;

public class CategoryApiMappings : Profile
{
    public CategoryApiMappings()
    {
        CreateMap<CategoryRequest, Category>();
        CreateMap<Category, CategoryResponse>();
    }
}