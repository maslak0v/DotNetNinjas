using AutoMapper;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Mapping
{
    public class RevenueMappingProfile : Profile
    {
        public RevenueMappingProfile()
        {
            CreateMap<Revenue, RevenueResponseDto>();
        }
    }
}
