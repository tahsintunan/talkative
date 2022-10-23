using FluentValidation;

namespace Application.Users.Commands.ForgetPassword
{
    public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Must provide an email");
        }
    }
}
