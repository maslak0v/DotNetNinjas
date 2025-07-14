using FinancialTracker.Services.Analytics.Models.Dto;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public static class BusMessageExtensions
{
    public static string GetInfo(this ITransactionCreatedMessage message)
    {
        var info = "";
        var messageType = message.GetType();
        foreach (var prop in messageType.GetProperties())
        {
            info += $"{prop.Name}={prop.GetValue(message)};";
        }

        return info;
    }
    public static ExpenseDto ToExpenseDto(this ITransactionCreatedMessage message)
    {
        return new ExpenseDto()
        {
            UserId = message.UserId,
            AccountId = message.AccountId,
            ExpenseId = message.TransactionId,
            Category = message.CategoryName,
            ExpenseTime = message.TransactionDate,
            Amount = message.Amount
        };
    }
}