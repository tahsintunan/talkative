using Application.Common.Interface;
using MediatR;

namespace Application.Tweets.Queries.SearchHashtags
{
    public class SearchHashtagsQuery : IRequest<SearchHashtagsVm>
    {
        public string? Hashtag { get; set; }
        public int? PageNumber { get; set; }
        public int? ItemCount { get; set; }
    }

    public class SearchHashtagsQueryHandler : IRequestHandler<SearchHashtagsQuery, SearchHashtagsVm>
    {
        private readonly ITweet _tweetService;

        public SearchHashtagsQueryHandler(ITweet tweetService)
        {
            _tweetService = tweetService;
        }

        public async Task<SearchHashtagsVm> Handle(
            SearchHashtagsQuery request,
            CancellationToken cancellationToken
        )
        {
            var pageNumber = request.PageNumber ?? 1;
            var itemCount = request.ItemCount ?? 20;

            var skip = (pageNumber - 1) * itemCount;
            var limit = pageNumber * itemCount;
            return new SearchHashtagsVm()
            {
                Hashtags = await _tweetService.SearchHashtags(request.Hashtag!, skip, limit)
            };
        }
    }
}
