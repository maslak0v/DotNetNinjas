using AutoMapper;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Mapping;

public class ExpenseMappingsProfile : Profile
{
    public ExpenseMappingsProfile()
    {
        CreateMap<Expense, ExpenseResponseDto>();
    }
}
