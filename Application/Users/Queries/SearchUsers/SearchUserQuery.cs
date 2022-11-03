using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Users.Queries.SearchUsers;

public class SearchUserQuery : IRequest<IList<UserVm>>
{
    public string? Username { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, IList<UserVm>>
{
    private readonly IMapper _mapper;
    private readonly IUser _userService;

    public SearchUserQueryHandler(IUser userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IList<UserVm>> Handle(
        SearchUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        var userList = await _userService.FindWithUsername(request.Username!, skip, limit);
        var userVmList = _mapper.Map<IList<UserVm>>(userList);
        return userVmList;
    }
}