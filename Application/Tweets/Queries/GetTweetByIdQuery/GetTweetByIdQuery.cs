using Application.Interface;
using Application.ViewModels;
using AutoMapper;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Domain.Entities;

namespace Application.Tweets.Queries.GetTweetByIdQuery
{
    public class GetTweetByIdQuery:IRequest<TweetVm?>
    {
        public string? Id { get; set; }
    }

    public class GetTweetByIdQueryHandler : IRequestHandler<GetTweetByIdQuery, TweetVm?>
    {
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _tweetBsonMapper;
        public GetTweetByIdQueryHandler(ITweetService tweetService, IBsonDocumentMapper<TweetVm> tweetBsonMapper)
        {
            _tweetService = tweetService;
            _tweetBsonMapper = tweetBsonMapper;
        }

        public async Task<TweetVm?> Handle(GetTweetByIdQuery request, CancellationToken cancellationToken)
        {
            var tweetBson = await _tweetService.GetTweetById(request.Id!);
            if(tweetBson == null)
            {
                return null;
            }
            var tweet = _tweetBsonMapper.map(tweetBson);
            return tweet;
        }

    }
}
