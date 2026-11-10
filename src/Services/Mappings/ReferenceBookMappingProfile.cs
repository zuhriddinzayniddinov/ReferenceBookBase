using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Services.Mappings;

public class ReferenceBookMappingProfile : Profile
{
    public ReferenceBookMappingProfile()
    {
        CreateMap<ReferenceBook, ReferenceBookDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ReverseMap();

        CreateMap<CreateReferenceBookDto, ReferenceBook>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<UpdateReferenceBookDto, ReferenceBook>()
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<ReferenceBookItem, ReferenceBookItemDto>()
            .ReverseMap();

        CreateMap<CreateReferenceBookItemDto, ReferenceBookItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ReferenceBook, opt => opt.Ignore());

        CreateMap<UpdateReferenceBookItemDto, ReferenceBookItem>()
            .ForMember(dest => dest.ReferenceBook, opt => opt.Ignore());
    }
}
