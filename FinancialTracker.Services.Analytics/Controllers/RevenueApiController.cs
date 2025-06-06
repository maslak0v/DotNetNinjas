using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers
{
    [ApiController]
    [Route("api/revenue")]
    public class RevenueApiController(IRevenuesService service, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromQuery] Guid userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var response = new ResponseDto();

            try
            {
                var revenue = await service.GetRevenuesAsync(userId, startDate, endDate);

                response.Result = mapper.Map<List<RevenueResponseDto>>(revenue);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("by-account")]
        public async Task<IActionResult> GetByAccountAsync([FromQuery] RevenuesRequestDto request)
        {
            var response = new ResponseDto();

            try
            {
                var revenue = await service.GetRevenuesByAccountAsync(request);
                response.Result = mapper.Map<List<RevenueResponseDto>>(revenue);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }
    }
}
