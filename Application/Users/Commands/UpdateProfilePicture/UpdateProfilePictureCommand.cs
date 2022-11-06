using System.Text.Json.Serialization;
using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Application.Users.Commands.UpdateProfilePicture
{
    public class UpdateProfilePictureCommand : IRequest
    {
        [JsonIgnore]
        public string? UserId { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }

    public class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand>
    {
        private readonly ICloudinary _cloudinary;
        private readonly IUser _userService;
        public UpdateProfilePictureCommandHandler(ICloudinary cloudinary, IUser userService)
        {
            _cloudinary = cloudinary;
            _userService = userService;
        }

        public async Task<Unit> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {

            var profilePictureUrl = await _cloudinary.UploadImage(request.ProfilePicture!);
            await _userService.PartialUpdate(request.UserId!, Builders<User>.Update.Set(x => x.ProfilePicture, profilePictureUrl));
            return Unit.Value;
        }
    }
}