using System;

namespace FinancialTracker.Services.Analytics.Models.Dto;

public class ExpensesRequestDto
{
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid AccountId { get; set; }
} 