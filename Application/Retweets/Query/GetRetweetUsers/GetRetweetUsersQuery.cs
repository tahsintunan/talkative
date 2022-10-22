using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Retweets.Query.GetRetweetUsers
{
    public class GetRetweetUsersQuery : IRequest<IList<UserVm>>
    {
        public string? OriginalTweetId { get; set; }
        public int? PageNumber { get; set; }
        public int? ItemCount { get; set; }
    }

    public class GetRetweetUsersQueryHandler : IRequestHandler<GetRetweetUsersQuery, IList<UserVm>>
    {
        private readonly IRetweetService _retweetService;

        public GetRetweetUsersQueryHandler(IRetweetService retweetService)
        {
            _retweetService = retweetService;
        }

        public async Task<IList<UserVm>> Handle(
            GetRetweetUsersQuery request,
            CancellationToken cancellationToken
        )
        {
            var pageNumber = request.PageNumber ?? 1;
            var itemCount = request.ItemCount ?? 20;

            var skip = (pageNumber - 1) * itemCount;
            var limit = pageNumber * itemCount;

            return await _retweetService.GetRetweetUsers(request.OriginalTweetId!, skip, limit);
        }
    }
}
