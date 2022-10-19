using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blocks.Queries.GetBlockedUsers
{
    public class GetBlockedUsersQuery:IRequest<IList<UserVm>>
    {
        public string? Id { get; set; }
    }

    public class GetBlockedUserQueryHandler : IRequestHandler<GetBlockedUsersQuery, IList<UserVm>>
    {
        private readonly IUserService _userService;
        public GetBlockedUserQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IList<UserVm>> Handle(GetBlockedUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetBlockedUsers(request.Id!);
        }
    }
}
