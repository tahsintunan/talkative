using Application.Common.Interface;
using MediatR;

namespace Application.Users.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest
{
    public string? Token { get; set; }
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IUser _userService;

    public ResetPasswordCommandHandler(IUser userService)
    {
        _userService = userService;
    }

    public async Task<Unit> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        await _userService.ResetPassword(request.Token!);

        return Unit.Value;
    }
}