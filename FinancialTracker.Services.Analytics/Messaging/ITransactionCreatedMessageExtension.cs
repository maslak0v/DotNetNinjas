using FinancialTracker.Services.Analytics.Models.Dto;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public static class IExpenseCreatedMessageExtension
{
    public static string GetInfo(this IExpenseCreatedMessage message)
    {
        var info = "";
        var messageType = message.GetType();
        foreach (var prop in messageType.GetProperties())
        {
            info += $"{prop.Name}={prop.GetValue(message)};";
        }

        return info;
    }
    public static ExpenseDto ToExpenseDto(this IExpenseCreatedMessage message)
    {
        return new ExpenseDto()
        {
            UserId = message.UserId,
            AccountId = message.AccountId,
            ExpenseId = message.ExpenseId,
            Category = message.CategoryName,
            ExpenseTime = message.ExpenseTime,
            Amount = message.Amount
        };
    }
}