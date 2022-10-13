using MediatR;

namespace Application.Retweets.Command.Retweet
{
    public class SimpleRetweetCommand:IRequest<string>
    {
        public string? RetweetId { get; set; }
    }

    public class SimpleRetweetCommandHandler : IRequestHandler<SimpleRetweetCommand, string>
    {
        public Task<string> Handle(SimpleRetweetCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
