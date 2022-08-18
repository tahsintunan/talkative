using FluentValidation;

namespace server.Dto.RequestDto.LoginRequestDto
{
    public class LoginRequestDtoValidator:AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(user => user.Username).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
