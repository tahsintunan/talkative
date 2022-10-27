using FluentValidation;

namespace Application.Comments.Commands.CreateComment;

public class CreateCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Text).NotNull().NotEmpty().WithMessage("Comment must have a body");
        RuleFor(x => x.TweetId).NotEmpty().NotNull().WithMessage("Tweet Id cannot be null");
    }
}