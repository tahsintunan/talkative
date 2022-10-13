using FluentValidation;

namespace Application.Common.Dto.ForgetPasswordDto
{
    public class ForgetPasswordDtoValidator : AbstractValidator<ForgetPasswordDto>
    {
        public ForgetPasswordDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty");
            //RuleFor(x=>x.Email).MustAsync(async (email, cancellation) =>
            //{
            //    var user = await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
            //    return user!=null;
            //}).WithMessage("User with current email doesn't exist");
        }
    }
}
