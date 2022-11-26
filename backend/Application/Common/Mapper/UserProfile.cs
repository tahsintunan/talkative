using Application.Common.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserVm>().ForMember(dest => dest.UserId!, opt => opt.MapFrom(src => src.Id!)).ReverseMap();
    }
}