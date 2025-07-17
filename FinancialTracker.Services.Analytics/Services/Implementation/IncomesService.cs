using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class IncomesService(IIncomesRepository incomesRepository, IMapper mapper) : IIncomesService
{
    public async Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await incomesRepository.GetIncomesAsync(userId, startDate, endDate);
    }

    public async Task<List<Income>> GetIncomesUpToDateAsync(Guid userId, DateTime upToDate)
    {
        return await incomesRepository.GetIncomesUpToDateAsync(userId, upToDate);
    }

    public async Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDTO request)
    {
        return await incomesRepository.GetIncomesByAccountAsync(request);
    }
    
    public async Task AddAsync(IncomeDto incomeDto)
    {
        var income = mapper.Map<Income>(incomeDto);
        await incomesRepository.AddAsync(income);
    }
}
