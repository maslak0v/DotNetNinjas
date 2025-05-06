using FinancialTracker.Services.Analytics.Models;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Controllers;

[ApiController]
[Route("api/expenses")]
public class ExpensesApiController(IExpensesService service) : ControllerBase
{
    [HttpGet]
    public IActionResult Get(
        [FromQuery] Guid userId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var response = new ResponseDto();
        
        try
        {
            var expenses = service.GetExpenses(userId, startDate, endDate);

            response.Result = expenses.Select(ExpenseToStr);
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return Ok(response);
    }

    private static string ExpenseToStr(Expense expense)
    {
        return $"{expense.Amount} рублей {expense.ExpenseTime:dd/MM/yy}";
    }
}