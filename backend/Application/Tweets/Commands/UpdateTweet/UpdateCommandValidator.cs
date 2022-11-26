using FluentValidation;

namespace Application.Tweets.Commands.UpdateTweet;

public class UpdateCommandValidator : AbstractValidator<UpdateTweetCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id cannot be null");
        RuleFor(x => x.Text).NotEmpty().NotNull().WithMessage("Tweet body cannot be empty");
    }
}