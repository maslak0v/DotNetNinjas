using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Advice;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Mapping;

public class AdviceMappingsProfile : Profile
{
    public AdviceMappingsProfile()
    {
        CreateMap<AdviceResult, AdviceResponseDto>();
    }
}