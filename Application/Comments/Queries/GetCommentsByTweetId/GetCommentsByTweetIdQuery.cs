using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Comments.Queries.GetCommentsByTweetId
{
    public class GetCommentsByTweetIdQuery : IRequest<IList<CommentVm>>
    {
        public string? TweetId { get; set; }
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
            return await _commentService.GetCommentsByTweetId(request.TweetId!);
        }
    }
}
