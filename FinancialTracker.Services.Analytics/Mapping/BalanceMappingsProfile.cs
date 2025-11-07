using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Mapping;

public class BalanceMappingsProfile : Profile
{
    public BalanceMappingsProfile()
    {
        CreateMap<decimal, BalanceResponseDto>()
            .ForMember(response => response.Amount, 
                opt => opt.MapFrom(src => src))
            .ForMember(response => response.Currency, opt => opt.Ignore());
    }
}