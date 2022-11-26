using System.Text.Json.Serialization;
using Application.Common.Enums;
using Application.Common.Interface;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Application.Users.Commands.UpdatePicture;

public class UpdatePictureCommand : IRequest
{
    [JsonIgnore] public string? UserId { get; set; }

    public IFormFile? Picture { get; set; }
    public PictureType? Type { get; set; }
}

public class UpdatePictureCommandHandler : IRequestHandler<UpdatePictureCommand>
{
    private readonly ICloudinary _cloudinary;
    private readonly IUser _userService;

    public UpdatePictureCommandHandler(ICloudinary cloudinary, IUser userService)
    {
        _cloudinary = cloudinary;
        _userService = userService;
    }

    public async Task<Unit> Handle(UpdatePictureCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserById(request.UserId!);
        if (request.Type == PictureType.ProfilePicture)
        {
            var previousProfilePicture = user!.ProfilePicture;
            var pictureUrl = await _cloudinary.UploadImage(request.Picture!, previousProfilePicture);
            await _userService.PartialUpdate(request.UserId!,
                Builders<User>.Update.Set(x => x.ProfilePicture, pictureUrl));
        }
        else if (request.Type == PictureType.CoverPicture)
        {
            var previousCoverPicture = user!.CoverPicture;
            var pictureUrl = await _cloudinary.UploadImage(request.Picture!, previousCoverPicture);
            await _userService.PartialUpdate(request.UserId!,
                Builders<User>.Update.Set(x => x.CoverPicture, pictureUrl));
        }

        return Unit.Value;
    }
}