using AutoMapper;
using Wallet.API.Models.Tags;
using Wallet.Application.Dto.Tags;
using Wallet.Domain.Entities;

namespace Wallet.API.Mappings;

public class TagApiMappings : Profile
{
    public TagApiMappings()
    {
        CreateMap<TagRequest, TagDto>().ReverseMap();
        CreateMap<TagDto, TagResponse>().ReverseMap();
        CreateMap<TagDto, Tag>().ReverseMap();
        CreateMap<TagDto, TagResponseDto>().ReverseMap();
    }
}