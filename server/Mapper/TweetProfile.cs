using AutoMapper;
using server.Dto.RequestDto.TweetRequestDto;
using server.Model.Tweet;
using server.ViewModels;

namespace server.Mapper
{
    public class TweetProfile:Profile
    {
        public TweetProfile()
        {
            CreateMap<TweetRequestDto, Tweet>().ReverseMap();
            CreateMap<Tweet,TweetVm>().ReverseMap();
        }
    }
}
