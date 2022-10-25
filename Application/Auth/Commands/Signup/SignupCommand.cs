using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using MongoDB.Bson;

namespace Application.Auth.Commands.Signup
{
    public class SignupCommand : IRequest
    {
        public string? Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }

    public class SignupCommandHandler : IRequestHandler<SignupCommand>
    {
        private readonly IAuth _authService;

        public SignupCommandHandler(IAuth authService)
        {
            _authService = authService;
        }

        public async Task<Unit> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            await _authService.SignupUser(
                new User()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Username = request.Username,
                    Password = request.Password,
                    DateOfBirth = request.DateOfBirth.Date,
                    Email = request.Email
                }
            );
            return Unit.Value;
        }
    }
}
