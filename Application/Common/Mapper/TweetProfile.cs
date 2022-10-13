using Application.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class TweetProfile : Profile
    {
        public TweetProfile()
        {
            CreateMap<Tweet, TweetVm>().ReverseMap();
        }
    }
}
