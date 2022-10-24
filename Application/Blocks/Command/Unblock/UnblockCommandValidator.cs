using FluentValidation;

namespace Application.Blocks.Command.Unblock
{
    public class UnblockCommandValidator : AbstractValidator<UnblockCommand>
    {
        public UnblockCommandValidator()
        {
            RuleFor(x => x.Blocked).NotNull().NotEmpty().WithMessage("Blocked id cannot be null");
        }
    }
}
