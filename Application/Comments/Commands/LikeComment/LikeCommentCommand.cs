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
        private readonly INotificationService _notificationService;

        public LikeCommentCommandHandler(IComment commentService, INotificationService notificationService)
        {
            _commentService = commentService;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(
            LikeCommentCommand request,
            CancellationToken cancellationToken
        )
        {
            var commentVm = await _commentService.GetCommentById(request.Id!);
            if (commentVm == null) { return Unit.Value; }

            var likes = new List<string>(commentVm.Likes!);
            
            if (likes.Contains(request.UserId!))
            {
                likes.Remove(request.UserId!);
            }
            else
            {
                likes.Add(request.UserId!);
                await _notificationService.TriggerLikeCommentNotification(request, commentVm);
            }

            await _commentService.PartialUpdate(
                request.Id!,
                Builders<Comment>.Update.Set(comment => comment.Likes, likes)
            );
            return Unit.Value;
        }
    }
}
