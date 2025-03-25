using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace pd311_web_api.BLL.Services.Image
{
    public class ImageService : IImageService
    {
        public bool DeleteImage(string imagePath)
        {
            try
            {
                var imagesPath = Path.Combine(Settings.RootPath, "wwwroot", Settings.RootImagesPath);
                imagePath = Path.Combine(imagesPath, imagePath);

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string?> SaveImageAsync(IFormFile image, string directoryPath)
        {
            try
            {
                var types = image.ContentType.Split("/");

                if (types[0] != "image")
                {
                    return null;
                }

                var imagesPath = Path.Combine(Settings.RootPath, "wwwroot", Settings.RootImagesPath, directoryPath);
                var imageName = $"{Guid.NewGuid()}.{types[1]}";

                var imagePath = Path.Combine(imagesPath, imageName);

                // Перевірка та створення директорії, якщо вона не існує
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                using (var stream = File.Create(imagePath))
                {
                    await image.CopyToAsync(stream);
                }

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Метод для збереження кількох зображень
        public async Task<List<string>> SaveImagesAsync(List<IFormFile> images, string directoryPath)
        {
            var imageNames = new List<string>();

            foreach (var image in images)
            {
                var imageName = await SaveImageAsync(image, directoryPath);
                if (imageName != null)
                {
                    imageNames.Add(imageName);
                }
            }

            return imageNames;
        }
    }
}
