using System.Text.Json.Serialization;
using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Comments.Queries.GetCommentsByTweetId;

public class GetCommentsByTweetIdQuery : IRequest<IList<CommentVm>>
{
    public string? TweetId { get; set; }
    public int? PageNumber { get; set; }
    public int? ItemCount { get; set; }
}

public class GetCommentsByTweetIdQueryHandler
    : IRequestHandler<GetCommentsByTweetIdQuery, IList<CommentVm>>
{
    private readonly IComment _commentService;

    public GetCommentsByTweetIdQueryHandler(IComment commentService)
    {
        _commentService = commentService;
    }

    public async Task<IList<CommentVm>> Handle(
        GetCommentsByTweetIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = request.PageNumber ?? 1;
        var itemCount = request.ItemCount ?? 20;

        var skip = (pageNumber - 1) * itemCount;
        var limit = pageNumber * itemCount;
        var commentVmList = await _commentService.GetCommentsByTweetId(request.TweetId!, skip, limit);
        return commentVmList;
    }
}