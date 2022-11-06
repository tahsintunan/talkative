using System.Text.Json.Serialization;
using Application.Common.Enums;
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
        public IFormFile? Picture { get; set; }
        public PictureType? Type { get; set; }
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

            var pictureUrl = await _cloudinary.UploadImage(request.Picture!);
            if (request.Type == PictureType.ProfilePicture)
            {
                await _userService.PartialUpdate(request.UserId!, Builders<User>.Update.Set(x => x.ProfilePicture, pictureUrl));
            }
            else if (request.Type == PictureType.CoverPicture)
            {
                await _userService.PartialUpdate(request.UserId!, Builders<User>.Update.Set(x => x.CoverPicture, pictureUrl));
            }

            return Unit.Value;
        }
    }
}