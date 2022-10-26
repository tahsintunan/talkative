using Application.Common.Interface;
using MediatR;

namespace Application.Blocks.Queries.GetBlockedUserIds
{
    public class GetBlockedUserIdsQuery : IRequest<Dictionary<string, bool>>
    {
        public string? UserId { get; set; }
    }

    public class GetBlockedUserIdsQueryHandler : IRequestHandler<GetBlockedUserIdsQuery, Dictionary<string, bool>>
    {
        private readonly IUser _userService;
        public GetBlockedUserIdsQueryHandler(IUser userService)
        {
            _userService = userService;
        }
        public async Task<Dictionary<string, bool>> Handle(GetBlockedUserIdsQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetBlockedUserIds(request.UserId!);
        }
    }
}