using AutoMapper;
using MessageBus.Shared.Contracts.Implementations;
using Wallet.Application.Dto.Transactions;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;

namespace Wallet.API.Mappings;

public class TransactionEventMappings : Profile
{
    public TransactionEventMappings()
    {
        CreateMap<TransactionDtoResponse, TransactionEvents.IncomeCreatedMessage>()
            .ConstructUsing(src => new TransactionEvents.IncomeCreatedMessage(
                Guid.NewGuid(),
                DateTime.UtcNow,
                src.TransactionId,
                src.UserId,
                src.AccountId,
                src.CategoryName,
                src.TransactionDate,
                src.Amount
            ));
        
        CreateMap<TransactionDtoResponse, TransactionEvents.ExpenseCreatedMessage>()
            .ConstructUsing(src => new TransactionEvents.ExpenseCreatedMessage(
                Guid.NewGuid(),
                DateTime.UtcNow,
                src.TransactionId,
                src.UserId,
                src.AccountId,
                src.CategoryName,
                src.TransactionDate,
                src.Amount
            ));

        CreateMap<TransactionDtoResponse, TransactionEvents.UpdateTransactionMessage>()
            .ConstructUsing(src => new TransactionEvents.UpdateTransactionMessage(
                Guid.NewGuid(),
                DateTime.UtcNow,
                src.TransactionId,
                src.UserId,
                src.AccountId,
                src.CategoryName,
                src.TransactionDate,
                src.Amount,
                src.OperationType.ToString(),
                src.OldOperationType.ToString()));

        CreateMap<DeleteTransactionDto, TransactionEvents.TransactionDeletedMessage>().ConstructUsing(src =>
            new TransactionEvents.TransactionDeletedMessage(
                Guid.NewGuid(),
                DateTime.UtcNow,
                src.TransactionId,
                src.OperationType.ToString()));
    }
}