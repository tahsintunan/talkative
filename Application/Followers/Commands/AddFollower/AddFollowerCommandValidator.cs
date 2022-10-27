using FluentValidation;

namespace Application.Followers.Commands.AddFollower;

public class AddFollowerCommandValidator : AbstractValidator<AddFollowerCommand>
{
    public AddFollowerCommandValidator()
    {
        RuleFor(x => x.FollowingId)
            .NotNull()
            .NotEmpty()
            .WithMessage("FollowingId cannot be null");
    }
}