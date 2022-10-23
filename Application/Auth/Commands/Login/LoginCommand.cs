using Application.Common.Interface;
using MediatR;

namespace Application.Auth.Commands.Login
{
    public class LoginCommand : IRequest<string?>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, string?>
    {
        private readonly IAuth _authService;

        public LoginCommandHandler(IAuth authService)
        {
            _authService = authService;
        }

        public async Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginUser(request.Username!, request.Password!);
        }
    }
}
