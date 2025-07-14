using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Mapping;

public class MessageMappingsProfile : Profile
{
    public MessageMappingsProfile()
    {
        CreateMap<IExpenseCreatedMessage, ExpenseDto>()
            .ForMember(dto => dto.Category, opt => opt.MapFrom(m => m.CategoryName));
    }
}