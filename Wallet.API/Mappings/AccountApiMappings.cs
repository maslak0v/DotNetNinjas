using AutoMapper;
using Wallet.API.Models.Accounts;
using Wallet.Application.Dto.Accounts;
using Wallet.Domain.Entities;

namespace Wallet.API.Mappings;

public class AccountApiMappings : Profile
{
    public AccountApiMappings()
    {
        CreateMap<AccountRequest, AccountDto>();

        CreateMap<AccountDto, AccountResponse>();

        CreateMap<UpdateAccountRequest, AccountDto>()
            .ForMember(a => a.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        
        CreateMap<AccountDto, Account>().ReverseMap();
    }
}