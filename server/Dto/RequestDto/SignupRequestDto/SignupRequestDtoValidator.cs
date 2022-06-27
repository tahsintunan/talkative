using FluentValidation;
using server.Interface;

namespace server.Dto.RequestDto.SignupRequestDto
{
    public class SignupRequestDtoValidator: AbstractValidator<SignupRequestDto>
    {
        private readonly IAuthService _authServices; 
        public SignupRequestDtoValidator(IAuthService authService)
        {
            _authServices = authService;

            RuleFor(user => user.Username).NotEmpty().WithMessage("Username cannot be empty");

            RuleFor(user => user.Email).NotEmpty().WithMessage("Email cannot be empty").EmailAddress().WithMessage("Not a proper email address");

            RuleFor(user => user.Password).Must(password =>
            {
                if (password == null)
                {
                    return false;
                }

                if (password.Length > 8 &&
                    password.Any(c => char.IsUpper(c)) &&
                    password.Any(c => char.IsLower(c)) &&
                    password.Any(c => char.IsDigit(c)) &&
                    (password.Any(c => char.IsSymbol(c)) || password.Any(c => char.IsPunctuation(c)))
                )
                {
                    return true;
                }

                return false;
            }).WithMessage("Password must contain more than 8 characters, at least 1 upper case letter, 1 digit and special character");

            RuleFor(user => user.DateOfBirth).NotEmpty().WithMessage("Date of birth cannot be empty").Must(dateOfBirth =>
            {
                var today = DateTime.Today;
                if (today.Year - dateOfBirth.Year < 18)
                {
                    return false;
                }
                return true;
            }).WithMessage("Must be above the age of 18");
        }

    }
    
}
