using MediatR;
using server.Application.Interface;

namespace Application.Tweets.Commands.DeleteTweetCommand
{
    public class DeleteTweetCommand:IRequest
    {
        public string? Id { get; set; }
    }

    public class DeleteTweetCommandHandler : IRequestHandler<DeleteTweetCommand>
    {
        private readonly ITweetService _tweetService;

        public DeleteTweetCommandHandler(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        public async Task<Unit> Handle(DeleteTweetCommand request, CancellationToken cancellationToken)
        {
            await _tweetService.DeleteTweet(request.Id!);
            return Unit.Value;
        }
    }
}
