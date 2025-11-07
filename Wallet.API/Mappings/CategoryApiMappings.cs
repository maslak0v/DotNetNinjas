using AutoMapper;
using Wallet.API.Models.Categories;
using Wallet.Application.Dto.Categories;
using Wallet.Domain.Entities;

namespace Wallet.API.Mappings;

public class CategoryApiMappings : Profile
{
    public CategoryApiMappings()
    {
        // CategoryRequest → CategoryDto (без даты обновления)
        CreateMap<CategoryRequest, CategoryDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // CategoryDto → Category
        CreateMap<CategoryDto, Category>()
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // <--- здесь
        
        CreateMap<CategoryRequest, CategoryUpdateDto>();
        CreateMap<CategoryUpdateDto, Category>()
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // <--- здесь
        
        // Category → CategoryResponse (без Transactions)
        CreateMap<CategoryDto, CategoryResponse>();
        
        CreateMap<Category, CategoryDto>();
    }
}
