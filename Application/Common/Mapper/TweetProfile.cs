using Application.Common.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapper;

public class TweetProfile : Profile
{
    public TweetProfile()
    {
        CreateMap<Tweet, TweetVm>().ReverseMap();
    }
}