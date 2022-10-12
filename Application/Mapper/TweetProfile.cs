using AutoMapper;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace server.Application.Mapper
{
    public class TweetProfile : Profile
    {
        public TweetProfile()
        {
            CreateMap<Tweet, TweetVm>().ReverseMap();
        }
    }
}


