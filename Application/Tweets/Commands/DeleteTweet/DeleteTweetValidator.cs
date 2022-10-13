using FluentValidation;

namespace Application.Tweets.Commands.DeleteTweet
{
    public class DeleteTweetValidator:AbstractValidator<DeleteTweetCommand>
    {
        public DeleteTweetValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id cannot be null");
        }
    }
}
