using FluentValidation;

namespace Application.Tweets.Commands.PublishTweet;

public class PublishTweetCommandValidator : AbstractValidator<PublishTweetCommand>
{
    public PublishTweetCommandValidator()
    {
        RuleFor(x => x.Text).NotEmpty().NotNull().WithMessage("Tweet doesn't have a body");
    }
}