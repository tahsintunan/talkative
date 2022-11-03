using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Users.Queries.GetTopActiveUsers
{
    public class GetTopActiveUsersQuery : IRequest<IList<UserVm>>
    {
        public int? PageNumber { get; set; }
        public int? ItemCount { get; set; }
    }

    public class GetTopActiveUsersQueryHandler : IRequestHandler<GetTopActiveUsersQuery, IList<UserVm>>
    {
        private readonly ITweet _tweetService;
        private readonly IMapper _mapper;
        public GetTopActiveUsersQueryHandler(ITweet tweetService, IMapper mapper)
        {
            _tweetService = tweetService;
            _mapper = mapper;
        }
        public async Task<IList<UserVm>> Handle(GetTopActiveUsersQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber ?? 1;
            var itemCount = request.ItemCount ?? 20;

            var skip = (pageNumber - 1) * itemCount;
            var limit = pageNumber * itemCount;
            var topUsers = await _tweetService.GetTopActiveUsers(skip, limit);
            IList<UserVm> topUserVmList = _mapper.Map<List<UserVm>>(topUsers);
            return topUserVmList;
        }
    }
}