using AutoMapper;
using FinancialTracker.Services.Analytics.DataAccess.Repositories;
using FinancialTracker.Services.Analytics.Models.Dto;
using FinancialTracker.Services.Analytics.Services;
using MassTransit;
using MessageBus.Shared.Contracts.Interfaces;

namespace FinancialTracker.Services.Analytics.Messaging;

public class ExpenseCreatedConsumer(
    ILogger<ExpenseCreatedConsumer> logger,
    IExpensesService expensesService,
    IMapper mapper) 
    : IConsumer<IExpenseCreatedMessage>
{
    public async Task Consume(ConsumeContext<IExpenseCreatedMessage> context)
    {
        var message = context.Message;
        
        logger.LogInformation($"[RabbitMQ] Expense created: UserId={message.UserId};" +
                              $"ExpenseId={message.ExpenseId}.\n" +
                              $"Info: {message.GetInfo()}"
                              );

        var expense = mapper.Map<ExpenseDto>(message);
        await expensesService.AddAsync(expense, CancellationToken.None);
    }
}