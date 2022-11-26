using FluentValidation;

namespace Application.Blocks.Command.Block;

public class BlockCommandValidator : AbstractValidator<BlockCommand>
{
    public BlockCommandValidator()
    {
        RuleFor(x => x.Blocked).NotEmpty().NotNull().WithMessage("Blocked id cannot be null");
    }
}