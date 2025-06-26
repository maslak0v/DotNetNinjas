namespace FinancialTracker.Services.Analytics.Models.Advice;

public class AdviceResult
{
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(Title)
               && string.IsNullOrEmpty(Message);
    }
}