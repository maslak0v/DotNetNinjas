using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation;

public class IncomesService(IIncomesRepository incomesRepository) : IIncomesService
{
    public async Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await incomesRepository.GetIncomesAsync(userId, startDate, endDate);
    }

    public async Task<List<Income>> GetIncomesBeforeDateAsync(Guid userId, DateTime beforeDate)
    {
        return await incomesRepository.GetIncomesBeforeDateAsync(userId, beforeDate);
    }

    public async Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDTO request)
    {
        return await incomesRepository.GetIncomesByAccountAsync(request);
    }
}
