using FinancialTracker.Services.Analytics.Models.Dto;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public static class IMessageExtension
{
    public static string GetInfo(this IMessage message)
    {
        var info = "";
        var messageType = message.GetType();
        foreach (var prop in messageType.GetProperties())
        {
            info += $"{prop.Name}={prop.GetValue(message)};";
        }

        return info;
    }
}