using AutoMapper;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Mapping;

public class IncomeMappingsProfile : Profile
{
    public IncomeMappingsProfile()
    {
        CreateMap<Income, IncomeResponseDto>();
        CreateMap<IncomeDto, Income>()
            .ForMember(e => e.Currency, opt => opt.Ignore())
            .ForMember(e => e.User, opt => opt.Ignore());
    }
}
