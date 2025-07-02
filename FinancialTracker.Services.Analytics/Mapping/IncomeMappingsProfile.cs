using AutoMapper;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Mapping;

public class IncomeMappingsProfile : Profile
{
    public IncomeMappingsProfile()
    {
        CreateMap<Income, IncomeResponseDTO>();
    }
}
