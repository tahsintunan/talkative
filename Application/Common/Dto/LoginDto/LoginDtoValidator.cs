using FluentValidation;

namespace Application.Dto.LoginDto
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(user => user.Username).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
