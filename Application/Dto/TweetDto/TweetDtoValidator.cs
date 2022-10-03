using FluentValidation;

namespace server.Application.Dto.TweetDto
{
    public class TweetDtoValidator : AbstractValidator<TweetDto>
    {
        public TweetDtoValidator()
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage("Tweet must have a body");
        }
    }
}
