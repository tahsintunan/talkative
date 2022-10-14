using FluentValidation;

namespace Application.Comments.Commands.UpdateComment
{
    public class UpdateCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Comment id cannot be null");
            RuleFor(x => x.Text).NotEmpty().NotNull().WithMessage("Comment must have a body");
        }
    }
}
