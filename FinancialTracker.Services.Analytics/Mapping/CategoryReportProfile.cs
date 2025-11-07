using AutoMapper;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Mapping;

public class CategoryReportProfile : Profile
{
    public CategoryReportProfile()
    {
        CreateMap<CategoryAggregate, CategoryReportItemDto>()
            .ConvertUsing(src =>
            new CategoryReportItemDto(src.Category, src.Total, src.Count));

        /*CreateMap<CategoryAggregate, CategoryReportItemDto>()
            .ConstructUsing(src => new CategoryReportItemDto(
                src.Category,
                src.Total,
                src.Count))
            .ForAllMembers(opt => opt.Ignore());*/
    }
}
