using Application.Common.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class CloudinaryService : ICloudinary
{
    private readonly Cloudinary cloudinary;

    public CloudinaryService(IConfiguration iconfig)
    {
        var cloudianryUrl = iconfig.GetValue<string>("Cloudinary:Url");
        cloudinary = new Cloudinary(cloudianryUrl);
        cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadImage(IFormFile file, string? previousPicture)
    {
        // ImageBase64 = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(c.Content))

        if (previousPicture != null)
        {
            var splittedImageUrl = previousPicture!.Split("/");
            var imageName = splittedImageUrl[splittedImageUrl.Count() - 1];
            var imageNameWithoutType = imageName.Split(".")[0];
            await cloudinary.DestroyAsync(new DeletionParams(imageNameWithoutType));
        }

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            QualityAnalysis = true,
            Transformation = new Transformation().Width(720)
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        return uploadResult.Url.AbsoluteUri;
    }
}