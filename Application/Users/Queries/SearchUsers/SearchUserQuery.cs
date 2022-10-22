using Application.Common.Interface;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Users.Queries.SearchUsers
{
    public class SearchUserQuery : IRequest<IList<UserVm>>
    {
        public string? Username { get; set; }
    }

    public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, IList<UserVm>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public SearchUserQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IList<UserVm>> Handle(
            SearchUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var userList = await _userService.FindWithUsername(request.Username!);
            var userVmList = _mapper.Map<IList<UserVm>>(userList);
            return userVmList;
        }
    }
}
