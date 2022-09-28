using AppCitas.Service.Entities;
using AppCitas.Service.Entities.DOTs;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AppCitas.Service.Helpers;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<AppUser, MemberDTO>();
		CreateMap<Photo, PhotoDTO >();
	}
}
