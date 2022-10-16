using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Followers.Queries.GetFollowers
{
    public class GetFollowersQuery:IRequest<IList<UserVm>>
    {
        public string? UserId { get; set; }
    }

    public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, IList<UserVm>>
    {
        private readonly IFollow _followerService;
        public GetFollowersQueryHandler(IFollow followerService)
        {
            _followerService = followerService;
        }

        public async Task<IList<UserVm>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
        {
            return await _followerService.GetFollowerOfSingleUser(request.UserId!);
        }
    }
}
