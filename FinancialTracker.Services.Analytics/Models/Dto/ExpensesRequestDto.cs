using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Models.Dto;

public class ExpensesRequestDto
{
    [FromQuery(Name = "userId")]
    public Guid UserId { get; set; }
    
    [FromQuery(Name = "startDate")]
    public DateTime StartDate { get; set; }
    
    [FromQuery(Name = "endDate")]
    public DateTime EndDate { get; set; }
    
    [FromQuery(Name = "accountId")]
    public Guid AccountId { get; set; }
} 