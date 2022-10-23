using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IList<UserVm>?>
    {
        public int? PageNumber { get; set; }
        public int? ItemCount { get; set; }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IList<UserVm>?>
    {
        private readonly IUser _userService;

        public GetAllUsersQueryHandler(IUser userService)
        {
            _userService = userService;
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
            return await _userService.GetAllUsers(skip, limit);
        }
    }
}
