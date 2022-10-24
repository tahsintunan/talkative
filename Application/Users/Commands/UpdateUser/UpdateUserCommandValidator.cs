using FluentValidation;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty()
                .NotNull()
                .WithMessage("Username cannot be null");
            RuleFor(user => user.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email cannot be empty")
                .EmailAddress()
                .WithMessage("Not a proper email address");

            RuleFor(user => user.DateOfBirth)
                .NotEmpty()
                .NotNull()
                .WithMessage("Date of birth cannot be empty")
                .Must(dateOfBirth =>
                {
                    var today = DateTime.Today;
                    return today.Year - dateOfBirth.Year >= 18;
                })
                .WithMessage("Must be above the age of 18");
        }
    }
}
