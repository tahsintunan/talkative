using Application.Common.Interface;
using Application.Common.ViewModels;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<CreateCommentCommandVm>
    {
        public string? UserId { get; set; }
        public string? TweetId { get; set; }
        public string? Text { get; set; }
    }

    public class CreateCommentCommandHandler
        : IRequestHandler<CreateCommentCommand, CreateCommentCommandVm>
    {
        private readonly IComment _commentService;
        private readonly ITweetService _tweetService;
        private readonly IBsonDocumentMapper<TweetVm> _mapper;

        public CreateCommentCommandHandler(
            IComment commentService,
            ITweetService tweetService,
            IBsonDocumentMapper<TweetVm> tweetMapper
        )
        {
            _commentService = commentService;
            _tweetService = tweetService;
            _mapper = tweetMapper;
        }

        public async Task<CreateCommentCommandVm> Handle(
            CreateCommentCommand request,
            CancellationToken cancellationToken
        )
        {
            var id = ObjectId.GenerateNewId().ToString();

            Comment comment = new Comment()
            {
                UserId = request.UserId,
                CreatedAt = DateTime.Now,
                TweetId = request.TweetId,
                Likes = new List<string>(),
                Id = id,
                Text = request.Text,
            };

            await _commentService.CreateComment(comment);

            var tweet = await _tweetService.GetTweetById(request.TweetId!);
            var tweetVm = _mapper.map(tweet!);

            tweetVm.Comments!.Add(id);

            await _tweetService.PartialUpdate(
                request.TweetId!,
                Builders<Tweet>.Update.Set(p => p.Comments, new List<string>(tweetVm.Comments!))
            );

            return new CreateCommentCommandVm() { Id = id };
        }
    }
}
