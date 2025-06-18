using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation
{
    public class IncomeService(IIncomesRepository incomeRepository) : IIncomesService
    {
        public async Task<List<Income>> GetIncomesAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await incomeRepository.GetIncomesAsync(userId, startDate, endDate);
        }

        public async Task<List<Income>> GetIncomesBeforeDateAsync(Guid userId, DateTime beforeDate)
        {
            return await incomeRepository.GetIncomesBeforeDateAsync(userId, beforeDate);
        }

        public async Task<List<Income>> GetIncomesByAccountAsync(IncomesRequestDto request) 
        {
            if(request.StartDate > request.EndDate)
                throw new ArgumentException("Дата начала периода не может быть позже даты окончания");

            return await incomeRepository.GetIncomesByAccountAsync(request);
        }
    }
}
