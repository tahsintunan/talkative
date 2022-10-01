using FluentValidation;

namespace server.Dto.RequestDto.TweetRequestDto
{
    public class TweetRequestDtoValidator:AbstractValidator<TweetRequestDto>
    {
        public TweetRequestDtoValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage("Tweet must have a body");
        }
    }
}
