using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class IncomesService(IIncomesRepository incomesRepository, IMapper mapper) : IIncomesService
{
    public async Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return await incomesRepository.GetIncomesAsync(userId, startDate, endDate, cancellationToken);
    }

    public async Task<decimal> GetSumOfIncomesUpToDateAsync(Guid userId, DateTime upToDate, CancellationToken cancellationToken)
    {
        return await incomesRepository.GetSumOfIncomesUpToDateAsync(userId, upToDate, cancellationToken);
    }

    public async Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDTO request, CancellationToken cancellationToken)
    {
        return await incomesRepository.GetIncomesByAccountAsync(request, cancellationToken);
    }
    
    public async Task AddAsync(IncomeDto incomeDto, CancellationToken cancellationToken)
    {
        var income = mapper.Map<Income>(incomeDto);
        await incomesRepository.AddAsync(income, cancellationToken);
    }
}
