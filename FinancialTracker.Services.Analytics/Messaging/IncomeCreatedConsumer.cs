using AutoMapper;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public class IncomeCreatedConsumer(
    ILogger<ExpenseCreatedConsumer> logger,
    IIncomesService incomesService,
    IMapper mapper) 
    : IConsumer<IIncomeCreatedMessage>
{
    public async Task Consume(ConsumeContext<IIncomeCreatedMessage> context)
    {
        var message = context.Message;
        
        logger.LogInformation($"[RabbitMQ] Expense created: UserId={message.UserId};" +
                              $"IncomeId={message.IncomeId}.\n" +
                              $"Info: {message.GetInfo()}"
        );

        var income = mapper.Map<IncomeDto>(message);
        await incomesService.AddAsync(income);
    }
}