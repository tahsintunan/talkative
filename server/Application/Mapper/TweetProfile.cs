using AutoMapper;
using server.Application.Dto.TweetDto;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace server.Application.Mapper
{
    public class TweetProfile : Profile
    {
        public TweetProfile()
        {
            CreateMap<TweetDto, Tweet>().ReverseMap();
            CreateMap<Tweet, TweetVm>().ReverseMap();
        }
    }
}
