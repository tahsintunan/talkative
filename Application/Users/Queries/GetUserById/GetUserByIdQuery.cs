using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserVm?>
{
    public string? UserId { get; set; }

    [JsonIgnore] public string? CurrentUser { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserVm?>
{
    private readonly IBlockFilter _blockFilter;
    private readonly IUser _userService;

    public GetUserByIdQueryHandler(IUser userService, IBlockFilter blockFilter)
    {
        _userService = userService;
        _blockFilter = blockFilter;
    }

    public async Task<UserVm?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userService.GetUserById(request.UserId!);
        var blockedUser = await _blockFilter.GetBlockedUserIds(request.CurrentUser!);
        if (user == null || _blockFilter.IsBlocked(user, blockedUser)) return null;

        return user;
    }
}