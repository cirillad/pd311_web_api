using Microsoft.AspNetCore.Http;

namespace pd311_web_api.BLL.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile image, string directoryPath);
        Task<List<string>> SaveImagesAsync(List<IFormFile> images, string directoryPath);
        bool DeleteImage(string imagePath);
    }
}