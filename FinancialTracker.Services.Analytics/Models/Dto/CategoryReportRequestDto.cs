using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Services.Analytics.Models.Dto;

public class CategoryReportRequestDto
{
    [FromQuery(Name = "userId")]
    public Guid UserId { get; set; }
    
    [FromQuery(Name = "startDate")]
    public DateTime StartDate { get; set; }
    
    [FromQuery(Name = "endDate")]
    public DateTime EndDate { get; set; }
}
