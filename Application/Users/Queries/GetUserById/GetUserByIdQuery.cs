using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserVm?>
    {
        public string? UserId { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserVm?>
    {
        private readonly IUser _userService;

        public GetUserByIdQueryHandler(IUser userService)
        {
            _userService = userService;
        }

        public async Task<UserVm?> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _userService.GetUserById(request.UserId!);
        }
    }
}
