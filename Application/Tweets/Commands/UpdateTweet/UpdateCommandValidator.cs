using FluentValidation;

namespace Application.Tweets.Commands.UpdateTweet
{
    public class UpdateCommandValidator:AbstractValidator<UpdateTweetCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x=>x.Id).NotEmpty().NotNull().WithMessage("Id cannot be null");
            RuleFor(x=>x.UserId).NotEmpty().NotNull().WithMessage("User not authenticated");
            RuleFor(x => String.IsNullOrEmpty(x.Text) && x.IsRetweet).Must(x => !x).WithMessage("Tweet body is empty");
        }
    }
}
