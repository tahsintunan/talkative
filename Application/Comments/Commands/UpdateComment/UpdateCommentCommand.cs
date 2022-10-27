using System.Text.Json.Serialization;
using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommand : IRequest
{
    public string? Id { get; set; }
    public string? Text { get; set; }

    [JsonIgnore] public string? UserId { get; set; }
}

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
{
    private readonly IComment _commentService;

    public UpdateCommentCommandHandler(IComment commentService)
    {
        _commentService = commentService;
    }

    public async Task<Unit> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var comment = await _commentService.GetCommentById(request.Id!);

        if (comment.UserId == request.UserId)
            await _commentService.PartialUpdate(
                request.Id!,
                Builders<Comment>.Update.Set(comment => comment.Text, request.Text)
            );
        return Unit.Value;
    }
}