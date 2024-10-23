using System.Reflection.Metadata;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser, MemberResponse>()
            .ForMember(dest => dest.Age, 
                opt => opt.MapFrom(
                    src => src.BirthDay.CalculateAge()))
            .ForMember(
                dest => dest.PhotoUrl, 
                opt => opt.MapFrom(
                    src => src.Photos.FirstOrDefault(p => p.IsMain)!.Url
                    ));
        CreateMap<Photo, PhotoResponse>();
    }
}