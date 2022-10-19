using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blocks.Command.Unblock
{
    public class UnblockCommand : IRequest
    {
        public string? BlockedBy { get; set; }
        public string? Blocked { get; set; }
    }

    public class UnblockCommandHandler : IRequestHandler<UnblockCommand>
    {
        private readonly IUserService _userService;

        public UnblockCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(UnblockCommand request, CancellationToken cancellationToken)
        {
            if (request.Blocked == request.BlockedBy)
                return Unit.Value;

            var blockedUser = await _userService.GetUserById(request.Blocked!);
            var blockedByUser = await _userService.GetUserById(request.BlockedBy!);
            if (blockedUser == null || blockedByUser == null)
                return Unit.Value;

            // blockedBy
            var blockedByList = blockedUser.BlockedBy;
            if (blockedByList != null)
            {
                blockedByList.Remove(request.BlockedBy);
                await _userService.PartialUpdate(
                    request.Blocked!,
                    Builders<User>.Update.Set(p => p.BlockedBy, new List<string>(blockedByList!))
                );
            }
            // blocked
            var blockedList = blockedByUser.Blocked;
            if (blockedList != null)
            {
                blockedList.Remove(request.Blocked);
                await _userService.PartialUpdate(
                    request.BlockedBy!,
                    Builders<User>.Update.Set(p => p.Blocked, new List<string>(blockedList!))
                );
            }
            return Unit.Value;
        }
    }
}
