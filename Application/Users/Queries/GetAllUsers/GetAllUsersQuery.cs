using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IList<UserVm>?>
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public int? PageNumber { get; set; }
        public int? ItemCount { get; set; }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IList<UserVm>?>
    {
        private readonly IUser _userService;
        private readonly IBlockFilter _blockFilter;

        public GetAllUsersQueryHandler(IUser userService, IBlockFilter blockFilter)
        {
            _userService = userService;
            _blockFilter = blockFilter;
        }

        public async Task<IList<UserVm>?> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken
        )
        {
            var pageNumber = request.PageNumber ?? 1;
            var itemCount = request.ItemCount ?? 20;

            var skip = (pageNumber - 1) * itemCount;
            var limit = pageNumber * itemCount;
            var users = await _userService.GetAllUsers(skip, limit);
            if (users == null) return null;
            return await _blockFilter.GetFilteredUsers(users, request.UserId!);
        }
    }
}
