using Application.Common.Interface;
using MediatR;

namespace Application.Followers.Queries.GetFollowingIdsOfCurrentUser
{
    public class GetFollowingIdsOfCurrentUserQuery : IRequest<Dictionary<string, bool>>
    {
        public string? UserId { get; set; }
    }

    public class GetFollowingIdsOfCUrrentUserQueryHandler
        : IRequestHandler<GetFollowingIdsOfCurrentUserQuery, Dictionary<string, bool>>
    {
        private readonly IFollow _followService;

        public GetFollowingIdsOfCUrrentUserQueryHandler(IFollow followService)
        {
            _followService = followService;
        }

        public Task<Dictionary<string, bool>> Handle(
            GetFollowingIdsOfCurrentUserQuery request,
            CancellationToken cancellationToken
        )
        {
            return _followService.GetFollowingOfCurrentUser(request.UserId!);
        }
    }
}
