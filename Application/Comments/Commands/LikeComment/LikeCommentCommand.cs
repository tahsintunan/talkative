using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Comments.Commands.LikeComment
{
    public class LikeCommentCommand : IRequest
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
    }

    public class LikeCommentCommandHandler : IRequestHandler<LikeCommentCommand>
    {
        private readonly IComment _commentService;

        public LikeCommentCommandHandler(IComment commentService)
        {
            _commentService = commentService;
        }

        public async Task<Unit> Handle(
            LikeCommentCommand request,
            CancellationToken cancellationToken
        )
        {
            var comment = await _commentService.GetCommentById(request.Id!);
            if (comment == null)
            {
                return Unit.Value;
            }

            var likes = new List<string>(comment.Likes!);

            if (likes.Contains(request.UserId!))
            {
                likes.Remove(request.UserId!);
            }
            else
            {
                likes.Add(request.UserId!);
            }

            await _commentService.PartialUpdate(
                request.Id!,
                Builders<Comment>.Update.Set(comment => comment.Likes, likes)
            );
            return Unit.Value;
        }
    }
}
