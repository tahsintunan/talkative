using FluentValidation;

namespace Application.Retweets.Command.Retweet
{
    public class RetweetCommandValidator : AbstractValidator<RetweetCommand>
    {
        public RetweetCommandValidator()
        {
            RuleFor(x => x.OriginalTweetId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Must provide the main tweet");
        }
    }
}
