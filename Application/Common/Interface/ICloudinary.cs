using Microsoft.AspNetCore.Http;

namespace Application.Common.Interface
{
    public interface ICloudinary
    {
        Task<string> UploadImage(IFormFile file);
    }
}