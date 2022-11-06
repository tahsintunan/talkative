using Application.Common.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinary
    {
        private Cloudinary cloudinary;
        public CloudinaryService(IConfiguration iconfig)
        {
            string cloudianryUrl = iconfig.GetValue<string>("Cloudinary:Url");
            cloudinary = new Cloudinary(cloudianryUrl);
            cloudinary.Api.Secure = true;

        }
        public async Task<string> UploadImage(IFormFile file)
        {
            // ImageBase64 = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(c.Content))

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.Url.AbsoluteUri;
        }
    }
}