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

    [HttpGet("by-account")]
    public IActionResult GetByAccount([FromQuery] ExpensesRequestDto request)
    {
        var response = new ResponseDto();
        
        try
        {
            var expenses = service.GetExpensesByAccount(request);
            response.Result = expenses.Select(expense => new
            {
                Amount = expense.Amount,
                Currency = expense.Currency,
                Date = expense.ExpenseTime.ToString("dd/MM/yyyy")
            });
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