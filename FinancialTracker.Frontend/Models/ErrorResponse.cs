namespace FinancialTracker.Frontend.Models;

public class ErrorResponse
{
    public string Message { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; }
}