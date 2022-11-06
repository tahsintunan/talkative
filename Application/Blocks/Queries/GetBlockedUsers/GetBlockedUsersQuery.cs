﻿using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Blocks.Queries.GetBlockedUsers;

public class GetBlockedUsersQuery : IRequest<IList<UserVm>>
{
    public string? UserId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetBlockedUserQueryHandler : IRequestHandler<GetBlockedUsersQuery, IList<UserVm>>
{
    private readonly IUser _userService;

    public GetBlockedUserQueryHandler(IUser userService)
    {
        _userService = userService;
    }

    public async Task<IList<UserVm>> Handle(
        GetBlockedUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        return await _userService.GetBlockedUsers(request.UserId!, skip, limit);
    }
}