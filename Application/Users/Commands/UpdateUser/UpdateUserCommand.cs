using Application.Common.Interface;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUser _userService;

        public UpdateUserCommandHandler(IUser userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await _userService.GetUserById(request.UserId!);
            if (user != null)
            {
                await _userService.UpdateUserInfo(
                    new User()
                    {
                        Id = request.UserId,
                        Username = request.Username,
                        DateOfBirth = request.DateOfBirth,
                        Email = request.Email
                    }
                );
            }

            return Unit.Value;
        }
    }
}
