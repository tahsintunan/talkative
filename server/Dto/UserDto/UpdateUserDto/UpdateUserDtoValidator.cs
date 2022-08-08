using FluentValidation;

namespace server.Dto.UserDto.UpdateUserDto
{
    public class UpdateUserDtoValidator:AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(user => user.Id).NotEmpty().WithMessage("Id cannot be null");
            RuleFor(user => user.Email).EmailAddress().WithMessage("Email is not valid");
            RuleFor(user => user.DateOfBirth).Must(dateOfBirth =>
            {
                var today = DateTime.Today;
                return !(today.Year - dateOfBirth?.Year < 18);
            }).WithMessage("Must be above the age of 18");
        }
    }
}
