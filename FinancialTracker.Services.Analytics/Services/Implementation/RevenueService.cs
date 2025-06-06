using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation
{
    public class RevenueService(IRevenuesRepository revenueRepository) : IRevenuesService
    {
        public async Task<List<Revenue>> GetRevenuesAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await revenueRepository.GetRevenuesAsync(userId, startDate, endDate);
        }

        public async Task<List<Revenue>> GetRevenuesBeforeDateAsync(Guid userId, DateTime beforeDate)
        {
            return await revenueRepository.GetRevenuesBeforeDateAsync(userId, beforeDate);
        }

        public async Task<List<Revenue>> GetRevenuesByAccountAsync(RevenuesRequestDto request) 
        {
            if(request.StartDate > request.EndDate)
                throw new ArgumentException("Дата начала периода не может быть позже даты окончания");

            return await revenueRepository.GetRevenuesByAccountAsync(request);
        }
    }
}
