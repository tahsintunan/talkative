using Application.Interface;
using Application.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Application.Retweet.Query.GetAllRetweetQuery
{
    public class GetAllRetweetQuery:IRequest<IList<TweetVm>>
    {
        public string? RetweetId { get; set; }
    }

    public class GetAllRetweetQueryHandler : IRequestHandler<GetAllRetweetQuery, IList<TweetVm>>
    {

        private readonly IRetweetService _retweetService;
        private readonly IBsonDocumentMapper<TweetVm> _documentMapper;
        public GetAllRetweetQueryHandler(IRetweetService retweetService, IBsonDocumentMapper<TweetVm> documentMapper)
        {
            _retweetService = retweetService;
            _documentMapper = documentMapper;
        }
        public async Task<IList<TweetVm>> Handle(GetAllRetweetQuery request, CancellationToken cancellationToken)
        {
            var retweetsBsonDocument = await _retweetService.GetAllRetweetPosts(request.RetweetId!);

            var retweets = retweetsBsonDocument["retweetPosts"].AsBsonArray;

            IList<TweetVm> tweetVm = new List<TweetVm>(); 

            foreach (var tweet in retweets)
            {
                tweetVm.Add(_documentMapper.map(tweet.AsBsonDocument));
            }

            return tweetVm;
        }
    }
}
