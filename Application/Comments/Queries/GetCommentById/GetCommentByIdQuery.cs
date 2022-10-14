using Application.Common.Interface;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Comments.Queries.GetCommentById
{
    public class GetCommentByIdQuery : IRequest<CommentVm>
    {
        public string? Id { get; set; }
    }

    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentVm>
    {
        private readonly IComment _commentService;

        public GetCommentByIdQueryHandler(IComment commentService)
        {
            _commentService = commentService;
        }

        public async Task<CommentVm> Handle(
            GetCommentByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _commentService.GetCommentById(request.Id!);
        }
    }
}
