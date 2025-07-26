using AutoMapper;
using Wallet.API.Models.Transactions;
using Wallet.Application.Dto.Transactions;
using Wallet.Domain.Entities;

namespace Wallet.API.Mappings;

public class TransactionApiMappings : Profile
{
     public TransactionApiMappings()
    {
        CreateMap<TransactionRequest, TransactionDto>();
        CreateMap<TransactionDto, Transaction>()
            .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.TagId, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Tag, opt => opt.Ignore());
        
        CreateMap<Transaction, TransactionDtoResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Tag != null ? src.Tag.Name : string.Empty))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Account != null ? src.Account.UserId : Guid.Empty));
        
        CreateMap<TransactionDtoResponse, TransactionResponse>()
            .ForMember(dest => dest.OperationType, opt => opt.MapFrom(src => src.OperationType))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.TagId))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate));
        
        CreateMap<Transaction, DeleteTransactionDto>()
            .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
            .ForMember(dest => dest.OperationType, opt => opt.MapFrom(src => src.OperationType));
    }
}
