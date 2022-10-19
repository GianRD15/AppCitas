using AppCitas.Service.Entities;
using AppCitas.Service.Entities.DOTs;
using AppCitas.Service.Extensions;
using AutoMapper;
using AppCitas.Service.DTOs;

namespace AppCitas.Service.Helpers;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<AppUser, MemberDTO>()
			.ForMember(
				dest => dest.PhotoUrl,
				opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
			.ForMember(
				dest => dest.Age,
				opt => opt.MapFrom(src => src.Birthday.CalculateAge()));

		CreateMap<Photo, PhotoDTO>();
		CreateMap<MemberUpdateDTO, AppUser>();

        CreateMap<RegisterDto, AppUser>();
    }
}
