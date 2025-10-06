namespace FinancialTracker.Services.Analytics.Models;

public sealed record CategoryAggregate(
    string Category,
    decimal Total,
    int Count);
