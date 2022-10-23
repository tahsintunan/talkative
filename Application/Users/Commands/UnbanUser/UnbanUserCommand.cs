using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Users.Commands.UnbanUser
{
    public class UnbanUserCommand : IRequest
    {
        public string? UserId { get; set; }
    }

    public class UnbanUserCommandHandler : IRequestHandler<UnbanUserCommand>
    {
        private readonly IUser _userService;

        public UnbanUserCommandHandler(IUser userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(
            UnbanUserCommand request,
            CancellationToken cancellationToken
        )
        {
            await _userService.PartialUpdate(
                request.UserId!,
                Builders<User>.Update.Set(x => x.IsBanned, false)
            );

            return Unit.Value;
        }
    }
}
