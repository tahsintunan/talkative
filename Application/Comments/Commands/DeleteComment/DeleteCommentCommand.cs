using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest
{
    public string? Id { get; set; }
}

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly IComment _commentService;
    private readonly IBsonDocumentMapper<TweetVm> _tweetMapper;
    private readonly ITweet _tweetService;

    public DeleteCommentCommandHandler(
        IComment commentService,
        ITweet tweetService,
        IBsonDocumentMapper<TweetVm> tweetMapper
    )
    {
        _commentService = commentService;
        _tweetService = tweetService;
        _tweetMapper = tweetMapper;
    }

    public async Task<Unit> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var comment = await _commentService.GetCommentById(request.Id!);

        if (comment == null) return Unit.Value;

        var tweetDoc = await _tweetService.GetTweetById(comment.TweetId!);

        if (tweetDoc != null)
        {
            var tweetVm = _tweetMapper.map(tweetDoc);

            var comments = new List<string>(tweetVm.Comments!);
            comments.Remove(request.Id!);

            await _tweetService.PartialUpdate(
                tweetVm!.Id!,
                Builders<Tweet>.Update.Set(p => p.Comments, comments)
            );
        }

        await _commentService.DeleteComment(request.Id!);
        return Unit.Value;
    }
}