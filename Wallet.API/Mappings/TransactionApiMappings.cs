using AutoMapper;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto.Transactions;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;

namespace Wallet.API.Mappings;

public class TransactionApiMappings : Profile
{
    public TransactionApiMappings()
    {
        CreateMap<TransactionRequest, TransactionDto>();
        CreateMap<TransactionDto, Transaction>()
            .ForMember(dest => dest.OperationType, opt => opt.MapFrom(src => (OperationType)src.OperationType))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
        CreateMap<Transaction, TransactionDtoResponse>()
            .ForMember(
                dest => dest.TransactionDate,
                opt => opt.MapFrom(src => src.TransactionDate)
            )
            .ForMember(
                dest => dest.OperationType,
                opt => opt.MapFrom(src => (byte)src.OperationType) 
            )
            .ForMember(
                dest => dest.Tag,
                opt => opt.MapFrom(src => src.TransactionTags
                    .Select(tt => tt.Tag.Name)
                    .FirstOrDefault())
            )
            .ReverseMap()
            .ForMember(
                dest => dest.TransactionDate,
                opt => opt.MapFrom(src => src.TransactionDate)
            )
            .ForMember(
                dest => dest.OperationType,
                opt => opt.MapFrom(src => (OperationType)src.OperationType)
            );

        CreateMap<TransactionDtoResponse, TransactionResponse>();
    }
}