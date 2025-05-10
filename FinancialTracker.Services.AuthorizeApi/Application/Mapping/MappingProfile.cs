using AutoMapper;
using FinancialTracker.Services.AuthorizeApi.Application.Contracts;
using FinancialTracker.Services.AuthorizeApi.Domain.Entities;

namespace FinancialTracker.Services.AuthorizeApi.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuthUser, UserResponse>();
            CreateMap<AuthUser, CurrentUserLoginResponse>();
            CreateMap<UserRegisterRequest, AuthUser>();
            CreateMap<UserLoginRequest, AuthUser>();
        }
    }
}
