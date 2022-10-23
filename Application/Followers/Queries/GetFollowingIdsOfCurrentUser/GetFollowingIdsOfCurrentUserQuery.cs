using Application.Common.Interface;
using MediatR;

namespace Application.Followers.Queries.GetFollowingIdsOfCurrentUser
{
    public class GetFollowingIdsOfCurrentUserQuery : IRequest<IList<string?>>
    {
        public string? UserId { get; set; }
    }

    public class GetFollowingIdsOfCUrrentUserQueryHandler
        : IRequestHandler<GetFollowingIdsOfCurrentUserQuery, IList<string?>>
    {
        private readonly IFollow _followService;

        public GetFollowingIdsOfCUrrentUserQueryHandler(IFollow followService)
        {
            _followService = followService;
        }

        public Task<IList<string?>> Handle(
            GetFollowingIdsOfCurrentUserQuery request,
            CancellationToken cancellationToken
        )
        {
            return _followService.GetFollowingOfCurrentUser(request.UserId!);
        }
    }
}
