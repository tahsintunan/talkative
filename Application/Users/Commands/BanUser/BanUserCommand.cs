using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Application.Users.Commands.BanUser
{
    public class BanUserCommand : IRequest
    {
        public string? UserId { get; set; }
    }

    public class BanUserCommandHandler : IRequestHandler<BanUserCommand>
    {
        private readonly IUser _userService;

        public BanUserCommandHandler(IUser userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.PartialUpdate(
                request.UserId!,
                Builders<User>.Update.Set(x => x.IsBanned, true)
            );

            return Unit.Value;
        }
    }
}
