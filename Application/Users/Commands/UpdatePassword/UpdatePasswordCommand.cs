using Application.Common.Interface;
using MediatR;

namespace Application.Users.Commands.UpdatePassword;

public class UpdatePasswordCommand : IRequest
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? UserId { get; set; }
}

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
{
    private readonly IUser _userService;

    public UpdatePasswordCommandHandler(IUser userService)
    {
        _userService = userService;
    }

    public async Task<Unit> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        await _userService.UpdatePassword(
            request.UserId!,
            request.OldPassword!,
            request.NewPassword!
        );
        return Unit.Value;
    }
}