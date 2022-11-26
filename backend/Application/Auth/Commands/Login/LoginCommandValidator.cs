using FluentValidation;

namespace Application.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password cannot be empty");
        RuleFor(x => x.Username).NotEmpty().NotNull().WithMessage("Username cannot be null");
    }
}