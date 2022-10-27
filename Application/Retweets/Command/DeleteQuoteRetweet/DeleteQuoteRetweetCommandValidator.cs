using FluentValidation;

namespace Application.Retweets.Command.DeleteQuoteRetweet;

public class DeleteQuoteRetweetCommandValidator : AbstractValidator<DeleteQuoteRetweetCommand>
{
    public DeleteQuoteRetweetCommandValidator()
    {
        RuleFor(x => x.OriginalTweetId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Must provide the original tweet");

        RuleFor(x => x.TweetId).NotEmpty().NotNull().WithMessage("Tweet cannot be null");
    }
}