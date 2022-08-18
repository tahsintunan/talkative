using FluentValidation;

namespace server.Dto.MessageDto;

public class MessageDtoValidator: AbstractValidator<MessageDto>
{
    public MessageDtoValidator()
    {
        RuleFor(x => x.SenderId ).NotEmpty().WithMessage("SenderId is required");
        RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("ReceiverId is required");
        RuleFor(x => x.MessageText).NotEmpty().WithMessage("Text is required");
    }
}