using AutoMapper;
using Wallet.API.Models.Categories;
using Wallet.Domain.Entities;

namespace Wallet.API.Mappings;

public class CategoryApiMappings : Profile
{
    public CategoryApiMappings()
    {
        CreateMap<CategoryRequest, Category>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<Category, CategoryResponse>();
    }
}