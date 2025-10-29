using AutoMapper;
using SafeSphere.API.DTOs;
using SafeSphere.API.Models;

namespace SafeSphere.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserResponseDto>();
        CreateMap<RegisterUserDto, User>();

        // PanicAlert mappings
        CreateMap<PanicAlert, PanicAlertResponseDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));
        CreateMap<CreatePanicAlertDto, PanicAlert>();

        // SOSAlert mappings
        CreateMap<SOSAlert, SOSAlertResponseDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));
        CreateMap<CreateSOSAlertDto, SOSAlert>();
    }
}

