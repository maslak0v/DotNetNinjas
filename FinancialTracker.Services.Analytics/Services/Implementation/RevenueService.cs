using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;

namespace FinancialTracker.Services.Analytics.Services.Implementation
{
    public class RevenueService(IRevenuesService revenuesService) : IRevenuesService
    {
        public async Task<List<Revenue>> GetRevenuesAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await revenuesService.GetRevenuesAsync(userId, startDate, endDate);
        }

        public async Task<List<Revenue>> GetRevenuesBeforeDateAsync(Guid userId, DateTime beforeDate)
        {
            return await revenuesService.GetRevenuesBeforeDateAsync(userId, beforeDate);
        }

        public async Task<List<Revenue>> GetRevenuesByAccountAsync(RevenuesRequestDto request) 
        {
            if(request.StartDate > request.EndDate)
                throw new ArgumentException("Дата начала периода не может быть позже даты окончания");

            return await revenuesService.GetRevenuesByAccountAsync(request);
        }
    }
}
