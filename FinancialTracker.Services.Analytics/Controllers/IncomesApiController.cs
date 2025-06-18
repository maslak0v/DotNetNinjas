using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers
{
    [ApiController]
    [Route("api/income")]
    public class IncomesApiController(IIncomesService service, IMapper mapper) : ControllerBase
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
                var income = await service.GetIncomesAsync(userId, startDate, endDate);

                response.Result = mapper.Map<List<IncomesResponseDto>>(income);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("by-account")]
        public async Task<IActionResult> GetByAccountAsync([FromQuery] IncomesRequestDto request)
        {
            var response = new ResponseDto();

            try
            {
                var income = await service.GetIncomesByAccountAsync(request);
                response.Result = mapper.Map<List<IncomesResponseDto>>(income);
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
