using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Comments.Queries.GetCommentById;

public class GetCommentByIdQuery : IRequest<CommentVm?>
{
    public string? Id { get; set; }

    [JsonIgnore] public string? UserId { get; set; }
}

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentVm?>
{
    private readonly IBlockFilter _blockFilterService;
    private readonly IComment _commentService;

    public GetCommentByIdQueryHandler(IComment commentService, IBlockFilter blockFilterService)
    {
        _commentService = commentService;
        _blockFilterService = blockFilterService;
    }

    public async Task<CommentVm?> Handle(
        GetCommentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var blockedUserList = await _blockFilterService.GetBlockedUserIds(request.UserId!);
        var commentVm = await _commentService.GetCommentById(request.Id!);

        if (commentVm == null || _blockFilterService.IsBlocked(commentVm, blockedUserList)) return null;
        return commentVm;
    }
}