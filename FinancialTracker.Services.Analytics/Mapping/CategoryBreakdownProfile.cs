using AutoMapper;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Mapping;

public class CategoryBreakdownProfile : Profile
{
    public CategoryBreakdownProfile()
    {
        CreateMap<CategoryAggregate, CategoryReportItemDto>();
    }
}
