using FluentValidation;

namespace Application.Tweets.Commands.PublishTweet
{
    public class PublishTweetValidator:AbstractValidator<PublishTweetCommand>
    {
        public PublishTweetValidator()
        {
            RuleFor(x => x.Text).NotEmpty().NotNull().WithMessage("Tweet doesn't have a body");
            RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("User isn't logged in");
        }
    }
}
