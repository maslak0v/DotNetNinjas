using AutoMapper;
using Wallet.API.Models.Accounts;
using Wallet.Domain.Entities;

namespace Wallet.API.Mappings;

public class AccountApiMappings : Profile
{
    public AccountApiMappings()
    {
        CreateMap<AccountRequest, Account>()
            .ForMember(a => a.Transactions, opt => opt.Ignore());

        CreateMap<Account, AccountResponse>();
        
        CreateMap<UpdateAccountRequest, Account>()
            .ForMember(a => a.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(a => a.Transactions, opt => opt.Ignore());
    }
}