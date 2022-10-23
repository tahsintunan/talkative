using Application.Common.Interface;
using MediatR;

namespace Application.Users.Commands.ForgetPassword
{
    public class ForgetPasswordCommand : IRequest
    {
        public string? Email { get; set; }
    }

    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand>
    {
        private readonly IUser _userService;

        public ForgetPasswordCommandHandler(IUser userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(
            ForgetPasswordCommand request,
            CancellationToken cancellationToken
        )
        {
            await _userService.ForgetPassword(request.Email!);
            return Unit.Value;
        }
    }
}
