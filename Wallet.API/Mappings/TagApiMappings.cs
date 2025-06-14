using AutoMapper;
using Wallet.API.Models.Tags;
using Wallet.Domain.Entities;

namespace Wallet.API.Mappings;

public class TagApiMappings : Profile
{
    public TagApiMappings()
    {
        CreateMap<TagRequest, Tag>();
        CreateMap<Tag, TagResponse>();
    }
}